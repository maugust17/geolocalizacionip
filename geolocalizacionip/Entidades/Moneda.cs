using geolocalizacionip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geolocalizacionip.Entidades
{
    class Moneda
    {
        

        public Moneda(string nombre, string codigoISO, string simbolo, double cotizacion)
        {
            Nombre = nombre;
            CodigoISO = codigoISO;
            Simbolo = simbolo;
            Cotizacion = cotizacion;
        }
        public Moneda()
        {
            Nombre = "";
            CodigoISO = "";
            Simbolo = "";
            Cotizacion = 0;
        }

       

        /// <summary>
        /// Nombre de la Moneda
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Código ISO de la Moneda. Se usan 3 Caracteres en Mayuscula.
        /// </summary>
        public string CodigoISO { get; set; }
        /// <summary>
        /// Indica la cotización en Dolares de la moneda seleccionada.
        /// </summary>
        public double Cotizacion { get; set; }
        /// <summary>
        /// Simbolo de la Moneda
        /// </summary>
        public string Simbolo { get; set; }
    }
}
