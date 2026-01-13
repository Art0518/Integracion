using System;

namespace Microservicio.Reserva.DTOs
{
 public class DisponibilidadResponse
 {
 public int IdMesa { get; set; }
 public DateTime Fecha { get; set; }
 public bool Disponible { get; set; }
 public string Mensaje { get; set; } = string.Empty;
 }
}
