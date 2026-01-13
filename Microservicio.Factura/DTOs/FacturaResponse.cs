namespace Microservicio.Factura.DTOs
{
    public class FacturaResponse
    {
        public int IdFactura { get; set; }
        public string IdReserva { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string TipoIdentificacion { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public string EstadoFactura { get; set; } = string.Empty;
        public string EstadoReserva { get; set; } = string.Empty;
        public DateTime FechaGeneracion { get; set; }
        public string UriFactura { get; set; } = string.Empty;
    }
}
