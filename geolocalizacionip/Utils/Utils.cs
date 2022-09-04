using Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geolocalizacionip
{
    public static class Utils
    {
        public static double ConvertiraDouble(string valor)
        {
            return Convert.ToDouble(valor.Replace(".", ","));

        }
        public static string ToStringNuevo(this Coordinate coordinate)
        {
            return string.Format($"({coordinate.Latitude}, {coordinate.Longitude})");
        }
    }
}
