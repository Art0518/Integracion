namespace Microservicio.Factura.DTOs
{
    public class FacturaRequest
    {
        public int IdReserva { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string TipoIdentificacion { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}
