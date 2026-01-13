namespace Microservicio.Factura.DTOs
{
 public class FacturaGetResponse
 {
 public int IdFactura { get; set; }
 public string IdReserva { get; set; } = string.Empty;
 public decimal Subtotal { get; set; }
 public decimal Iva { get; set; }
 public decimal Total { get; set; }
 public string EstadoFactura { get; set; } = string.Empty;
 public DateTime FechaGeneracion { get; set; }
 public string UriFactura { get; set; } = string.Empty;
 }
}
