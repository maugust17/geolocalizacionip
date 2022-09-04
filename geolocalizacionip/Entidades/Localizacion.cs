using Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geolocalizacionip.Entidades
{
    internal class Localizacion
    {
        private readonly Coordinate buenosAires = new Coordinate(-34, -64);
        public Localizacion(Coordinate origen, Coordinate destino)
        {
            Origen = origen;
            Destino = destino;
        }
        public Localizacion(Coordinate destino)
        {
            Origen = buenosAires;
            Destino = destino;
        }

        public Coordinate Origen { get; set; }
        public Coordinate Destino { get; set; }
        public double Distancia { get { return GeoCalculator.GetDistance(Origen, Destino, 0, DistanceUnit.Kilometers); } }
    }
}
