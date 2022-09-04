using geolocalizacionip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace geolocalizacionip.Rest
{
    public class CotizacionRest
    {
        private HttpClient client = new HttpClient();
        private string URL { get { return "https://api.apilayer.com/fixer/latest"; } }
        public CotizacionRest()
        {

        }
        public async Task<CotizacionResponse> Request()
        {
            CotizacionResponse listaCotizaciones = new CotizacionResponse();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("apikey", "PkR29Lo7bjXjzc0Igm5dHqYtbyoNbCXg");
            // List data response.
            HttpResponseMessage response = await client.GetAsync("?base=USD");  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var json = await response.Content.ReadAsStringAsync();  //Make sure to add a reference to System.Net.Http.Formatting.dll

                var salida = JsonSerializer.Deserialize<CotizacionResponse>(json);

                if (salida != null)
                    listaCotizaciones = salida;

            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return listaCotizaciones;
        }
    }
}
