namespace estadisticasapi
{
    public class Estadisticas
    {
        public string Referencia { get { return "Buenos Aires"; } }
        public string Unidad { get { return "km"; } }
        public string DistanciaMinima { get; set; }
        public string DistanciaMaxima { get; set; }
        public string DistanciaPromedio { get; set; }
    }
}
