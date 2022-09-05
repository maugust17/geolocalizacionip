using Geolocation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geolocalizacionip.Entidades
{
    /// <summary>
    /// Clase para recolectar toda la info traida de las APIs o db y generar una respuesta.
    /// </summary>
    internal class InfoIP
    {
        public string IP { get; set; }
        public DateTime HoraActual { get; set; }
        public Pais Pais { get; set; }
        public List<Idioma> Idiomas { get; set; }
        public List<Moneda> Monedas { get; set; }
        public Localizacion Localizacion { get; set; }
        public List<string> ZonasHorarias { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"IP: {IP}, fecha actual:{HoraActual.ToString("YYYY/MM/dd HH:mm:ss")}");
            sb.AppendLine($"País: {Pais.Nombre} ({Pais.NombreIngles.ToLower()})");
            sb.AppendLine($"Código ISO3166-1 alpha2:{Pais.CodigoISO}");
            sb.AppendLine($"Código ISO3166-1 alpha3:{Pais.CodigoISO3}");
            
            sb.Append($"Idiomas: ");
            foreach (Idioma idioma in Idiomas)
            {
                sb.Append($"{idioma.Nombre} ({idioma.CodigoISO}) ");
            }
            sb.AppendLine($"");
            sb.Append($"Moneda: ");
            foreach (Moneda moneda in Monedas)
            {
                sb.Append($"{moneda.CodigoISO} (1 {moneda.CodigoISO} = {String.Format("{0:0.00000}",moneda.Cotizacion)} USD) ");
            }
            sb.AppendLine($"");
            sb.Append($"Hora: ");
            foreach (string zonahoraria in ZonasHorarias)
            {
                //Ejemplos
                //zonahoraria = UTC+03:00
                //zonahoraria = UTC-02:00
                //zonahoraria = UTC
                DateTime dateTime = HoraActual.ToUniversalTime();

                if (zonahoraria.Length == 9)
                {
                    int horas = Convert.ToInt32(zonahoraria.Substring(4, 2));
                    int minutos = Convert.ToInt32(zonahoraria.Substring(7, 2));
                    TimeSpan ts;
                    if (zonahoraria[3] == '+')
                    {
                        ts = new TimeSpan(horas, minutos, 0);
                        
                        
                    }
                    else//(zonahoraria[3] == '-')
                    {
                        ts = new TimeSpan(-1 * horas, minutos, 0);
                         
                    }
                    dateTime += ts;
                }

                sb.Append($"{dateTime.ToString()} ({zonahoraria}) ");
            }
            sb.AppendLine($"");
            sb.AppendLine($"Distancia Estimada: {Localizacion.Distancia} Km {Localizacion.Origen.ToStringNuevo()} a {Localizacion.Destino.ToStringNuevo()}");

            return sb.ToString(); ;

        }
        
    }
}
