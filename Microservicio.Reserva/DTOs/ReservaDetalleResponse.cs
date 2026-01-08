namespace Microservicio.Reserva.DTOs
{
    public class ReservaDetalleResponse
    {
        public string Mensaje { get; set; } = string.Empty;
        public string IdReserva { get; set; } = string.Empty;
        public string IdMesa { get; set; } = string.Empty;
        public string Fecha { get; set; } = string.Empty;
        public int NumeroPersonas { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string TipoMesa { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public string ApellidoCliente { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public decimal ValorPagado { get; set; }
        public string UriFactura { get; set; } = string.Empty;
    }
}
