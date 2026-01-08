using System;

namespace Microservicio.Reserva.DTOs
{
    public class PreReservaBusResponse
    {
        public string IdMesa { get; set; } = string.Empty;
    public DateTime FechaReserva { get; set; }
      public int NumeroPersonas { get; set; }
    public int DuracionHoldSegundos { get; set; }
     public string IdHold { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
    }
}
