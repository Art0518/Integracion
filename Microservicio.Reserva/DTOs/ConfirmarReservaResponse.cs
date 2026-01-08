namespace Microservicio.Reserva.DTOs
{
    public class ConfirmarReservaResponse
 {
        public string Mensaje { get; set; } = string.Empty;
      public string IdReserva { get; set; } = string.Empty;
   public string IdMesa { get; set; } = string.Empty;
    public string NombreCliente { get; set; } = string.Empty;
  public string ApellidoCliente { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
      public string TipoIdentificacion { get; set; } = string.Empty;
 public string Identificacion { get; set; } = string.Empty;
        public string FechaReserva { get; set; } = string.Empty;
        public int NumeroPersonas { get; set; }
        public decimal ValorPagado { get; set; }
        public string UriFactura { get; set; } = string.Empty;
    }
}
