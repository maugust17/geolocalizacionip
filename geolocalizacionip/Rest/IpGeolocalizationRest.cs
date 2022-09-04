using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using geolocalizacionip.Models;
using System.Net.Http.Headers;

namespace geolocalizacionip.Rest
{
    public class IpGeolocalizationRest
    {
        private HttpClient client = new HttpClient();
        private string URL { get { return "http://api.ipstack.com"; } }
        public IpGeolocalizationRest()
        { 
           
        }
        public async Task<String> Request(string ip)
        {
            string codigoPais = "";
            var parameters = string.Format($"/{ip}?access_key=e23dbeaae139b098a1a37b7afc7d13c6&fields=country_code");
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = await client.GetAsync(parameters);  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var json = await response.Content.ReadAsStringAsync();  //Make sure to add a reference to System.Net.Http.Formatting.dll

                var salida = JsonSerializer.Deserialize<IpResponse>(json);

                if (salida != null)
                    codigoPais = salida.country_code;

            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return codigoPais;
        }
       
    }
}
