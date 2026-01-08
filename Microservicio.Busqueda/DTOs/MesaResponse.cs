namespace Microservicio.Busqueda.DTOs
{
    public class MesaResponse
    {
        public int IdMesa { get; set; }
        public int IdRestaurante { get; set; }
        public int NumeroMesa { get; set; }
        public string TipoMesa { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public decimal Precio { get; set; }
        public string ImagenURL { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
