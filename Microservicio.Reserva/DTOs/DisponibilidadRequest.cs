namespace Microservicio.Reserva.DTOs
{
 public class DisponibilidadRequest
 {
 public string id_mesa { get; set; } = string.Empty;
 public string fecha { get; set; } = string.Empty;
 public int numeroPersonas { get; set; }
 }
}
