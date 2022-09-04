using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geolocalizacionip.Entidades
{
    internal class Pais
    {
        public Pais()
        {
            Nombre = "";
            NombreIngles = ""; 
            CodigoISO = "";
            CodigoISO3 = "";
        }

        public Pais(string nombre, string nombreIngles, string codigoIso, string codigoIso3)
        {
            Nombre = nombre;
            NombreIngles = nombreIngles;
            CodigoISO = codigoIso;
            CodigoISO3 = codigoIso3;
        }

        /// <summary>
        /// Nombre de País
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Nombre de País
        /// </summary>
        public string NombreIngles { get; set; }
        /// <summary>
        /// Código ISO-3166-1 alpha2 del páís. 2 Letras Mayusculas.
        /// </summary>
        public string CodigoISO { get; set; }
        /// <summary>
        /// Código ISO-3166-1 alpha3 del páís. 3 Letras Mayusculas.
        /// </summary>
        public string CodigoISO3 { get; set; }
    }
}
