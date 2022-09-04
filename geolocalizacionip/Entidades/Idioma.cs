using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geolocalizacionip.Entidades
{
    internal class Idioma
    {
       
        public Idioma()
        {
            Nombre = "";
            CodigoISO = "";
        }

        public Idioma(string key, string value)
        {
            this.CodigoISO = key;
            this.Nombre = value;
        }

        /// <summary>
        /// Nombre del Idioma.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Código ISO 639-1 del Idioma. 2 Letras minusculas..
        /// </summary>
        public string CodigoISO { get; set; }
    }
}
