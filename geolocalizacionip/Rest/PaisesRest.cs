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
    public class PaisesRest 
    {
        private HttpClient client = new HttpClient();
        private string URL { get { return "https://restcountries.com/v3.1/all"; } }
        public PaisesRest() 
        {

        }
        public async Task<List<PaisResponse>> Request()
        {
            List<PaisResponse> listaPaises = new List<PaisResponse>();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = await client.GetAsync("?fields=name,latlng,currencies,languages,timezones,cca2,cca3,translations");  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var json = await response.Content.ReadAsStringAsync();  //Make sure to add a reference to System.Net.Http.Formatting.dll

                var salida = JsonSerializer.Deserialize<List<PaisResponse>>(json);

                if (salida != null)
                    listaPaises = salida;

            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return listaPaises;
        }
      
    }
}
