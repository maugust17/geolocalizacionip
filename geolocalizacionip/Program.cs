using StackExchange.Redis;
using geolocalizacionip.Rest;
using System.Text.Json;
using geolocalizacionip.Models;
using geolocalizacionip.Entidades;
using System.Linq;
using Geolocation;
using ServiceStack.Redis;
using System.Net;

namespace geolocalizacionip
{
    internal class Program
    {
        static TimeSpan tsDia = new TimeSpan(1, 0, 0, 0);
        static TimeSpan tsMes = new TimeSpan(30, 0, 0, 0);
        //DB con la información completa por país(moneda, zon horaria, idiomas, etc)
        static IDatabase paisDB = null;
        //DB con las cotizaciones por moneda
        static IDatabase cotizacionDB = null;
        //DB con el Código ISO de país por IP
        static IDatabase ipDB = null;
        //DB con la 3 registros para el máximo, minimo y promedio
        static IDatabase estadisticasDB = null;
        static async Task Main(string[] args)
        {
            
            string ip = args[0];

            if (!ValidarIP(ip))
            {
                Console.WriteLine("Error en el formato de la IP!!!");
                return;
            }
            //ip = "104.41.63.254";//Brasil
            //ip = "83.44.196.93";//España
            //ip = "2800:810:508:8a19:1050:d989:3b38:fd6c";//Argentina

                //Guardo la Hora de la consulta.
            DateTime horaActual = DateTime.Now;



            //Inicio la conexión a Redis
            try
            {
                paisDB = RedisDB.PaisDB;
                cotizacionDB = RedisDB.CotizacionDB;
                ipDB = RedisDB.IpDB;
                estadisticasDB = RedisDB.EstadisticasDB;

            }
            catch (Exception e)
            {
                Console.WriteLine("Error al conectarse con el motor Redis. Revise los parámetros de conexión.");
                return;
            }

            //Actualizo la lista de cotizaciones solo si no existen en Redis.
            await actualizarCotizaciones();

            //Obtengo el codigo de país de la IP
            string codigoPaisIP = await obtenerCodigoPaisPorIP(ip);

            //Obtengo los datos del código de pais
            PaisResponse infoPais = await obtenerDatosPais(codigoPaisIP);

            //Mapeo los valores a un InfoIP para poder imprimirlos mas facilmente
            InfoIP infoIP = mapearValoresPais(ip, infoPais, horaActual);

            //Actualizo valores de distancia mínima, maxima y promedio.
            string minimo = await estadisticasDB.StringGetAsync("minimo");
            string maximo = await estadisticasDB.StringGetAsync("maximo");
            string promedioStr = await estadisticasDB.StringGetAsync("promedio");

            await estadisticasDB.StringIncrementAsync("lecturas", 1);

            long lecturas = Convert.ToInt64 (await estadisticasDB.StringGetAsync("lecturas"));

            if (minimo == null || infoIP.Localizacion.Distancia < Utils.ConvertiraDouble(minimo))
                await estadisticasDB.StringSetAsync("minimo", infoIP.Localizacion.Distancia);

            if (maximo == null || infoIP.Localizacion.Distancia > Utils.ConvertiraDouble(maximo))
                await estadisticasDB.StringSetAsync("maximo", infoIP.Localizacion.Distancia);

            if (promedioStr == null)
            {
                await estadisticasDB.StringSetAsync("promedio", infoIP.Localizacion.Distancia);
            }
            else
            {
                double promedio = Utils.ConvertiraDouble(promedioStr);
                await estadisticasDB.StringSetAsync("promedio", (double)(promedio + infoIP.Localizacion.Distancia)/lecturas);
            }
            Console.WriteLine(infoIP.ToString());
        }

        private static bool ValidarIP(string ip_validar)
        {
            IPAddress ip;
            return IPAddress.TryParse(ip_validar, out ip);
        }

        /// <summary>
        /// Mapea los datos de diferentes objetos en un InfoIP.
        /// </summary>
        /// <param name="ip">String con la ip a guardar.</param>
        /// <param name="infoPais">Objeto PaisResponse obtenido de Redis o el Webservice.</param>
        /// <param name="horaActual">Hora en la cual se hizo la consulta.</param>
        /// <returns>Un objeto InfoIP mapeado con todos los datos necesarios para mostrar en pantalla.</returns>
        private static InfoIP mapearValoresPais(string ip, PaisResponse infoPais, DateTime horaActual)
        {

            InfoIP infoIP = new InfoIP();

            infoIP.IP = ip;
            infoIP.Idiomas = infoPais.languages.Select(x => new Idioma(x.Key, x.Value)).ToList<Idioma>();
            infoIP.Localizacion = new Localizacion(new Coordinate(infoPais.latlng[0], infoPais.latlng[1]));
            infoIP.HoraActual = horaActual;
            infoIP.Monedas = infoPais.currencies.Select(x => new Moneda(x.Value.name, x.Key, x.Value.symbol, 1.0 / Utils.ConvertiraDouble(cotizacionDB.StringGet(x.Key.ToLower()).ToString()))).ToList<Moneda>();
            infoIP.Pais = new Pais(infoPais.translations["spa"].common, infoPais.name.common, infoPais.cca2, infoPais.cca3);
            infoIP.ZonasHorarias = infoPais.timezones;

            return infoIP;
        }

        /// <summary>
        /// Actualiza la lista de cotizaciones desde el webservice si Redis esta vacio. Los registros expiran cada 1 día.
        /// </summary>
        /// <returns>No retorna nada.</returns>
        public static async Task actualizarCotizaciones()
        {
            //Busco la cotización del dolar, que siempre es 1 para ver si tengo la lista cargada en Redis.
            string cotizacionDolar = await cotizacionDB.StringGetAsync("usd");
            if (cotizacionDolar == null)
            {
                //Si no existe la cotización actualizo la lista completa desde el WebService y la vuelco en redis..
                CotizacionRest cotizacionRest = new CotizacionRest();
                CotizacionResponse cotizaciones = await cotizacionRest.Request();
                foreach (KeyValuePair<string, double> conversion in cotizaciones.rates)
                {
                    //Actualizo Redis con la nueva cotización. Los valores expiran en 1 día.
                    await cotizacionDB.StringSetAsync(conversion.Key.ToLower(), conversion.Value, tsDia); ;
                }

            }
        }

        /// <summary>
        /// Busca los datos asociados al país a traves del código iso alpha3. Por defecto usca en redis, si no existe actualiza la lista completa desde el WebService.
        /// </summary>
        /// <param name="codigoPaisIP">Código ISO-3166-1 alpha3, en mayusculas.</param>
        /// <returns>Retorn un objeto con la información solicitada del país.</returns>
        public static async Task<PaisResponse> obtenerDatosPais(string codigoPaisIP)
        {
            string jsonPais = await paisDB.StringGetAsync(codigoPaisIP.ToLower());
            if (jsonPais == null)
            {
                var obtenerPaises = new PaisesRest();
                Task<List<PaisResponse>> consultaPaises = obtenerPaises.Request();

                List<PaisResponse> listaPaises = await consultaPaises;
                foreach (PaisResponse pais in listaPaises)
                {
                    //Guardo la info de cada país. Expira en 1 mes.
                    await paisDB.StringSetAsync(pais.cca2.ToLower(), JsonSerializer.Serialize(pais), tsMes);
                }
                jsonPais = await paisDB.StringGetAsync(codigoPaisIP.ToLower());

            }
            //Convierto el Json obtenido en un objeto
            return JsonSerializer.Deserialize<PaisResponse>(jsonPais);
        }

        /// <summary>
        /// Realiza la busqueda de la IP, primero en Redis y si no existe a traves del WebService. Las IP se guardan en redis por 1 día.
        /// </summary>
        /// <param name="ip">IP a buscar, puede ser IPv4 o IPv6</param>
        /// <returns>Retorna el Código ISO de país.</returns>
        /// <exception cref="Exception"></exception>
        public static async Task<string> obtenerCodigoPaisPorIP(string ip)
        {
            string codigoPaisIP = await ipDB.StringGetAsync(ip.ToLower());
            if (codigoPaisIP == null)
            {
                var obtenerIP = new IpGeolocalizationRest();
                Task<string> consultaIP = obtenerIP.Request(ip);

                //Espero la respuesta de la API de Geolocalización
                try
                {
                    codigoPaisIP = await consultaIP;
                }
                catch (Exception e)
                {
                    //Si no puedo obtener los datos de la ip de ninguna forma, lanzo una excecion para terminar la aplicación.
                    Console.WriteLine("Error al obtener la IP, no existe en Redis y el servicio no responde.");
                    throw new Exception("Error al obtener la IP, no existe en Redis y el servicio no responde.");
                }
                //Actualizo Redis con la ip y el código de país, expira en 1 día.
                await ipDB.StringSetAsync(ip.ToLower(), codigoPaisIP, tsDia);

            }
            return codigoPaisIP;
        }
        
    }


}