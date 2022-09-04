using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geolocalizacionip.Models
{
    public class Name
    {
        public string common { get; set; }
        public string official { get; set; }
        //public Dictionary<string, NativeName> nativeName { get; set; } = new();

    }
    public class Currency
    {
        public string name { get; set; }
        public string symbol { get; set; }
    }
    public class PaisResponse
    {
        public Name name { get; set; }
        public Dictionary<string, Currency> currencies { get; set; } = new();

        public string cca2 { get; set; }
        public string cca3 { get; set; }
        public Dictionary<string, string> languages { get; set; } = new();
        public Dictionary<string, Name> translations { get; set; } = new();
        public List<double> latlng { get; set; } = new();
        public List<string> timezones { get; set; } = new();
        
    }
}
