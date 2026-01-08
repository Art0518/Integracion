using System;

namespace Microservicio.Reserva.DTOs
{
    public class PreReservaBusDTO
    {
        public string id_mesa { get; set; } = string.Empty;
        public DateTime fecha { get; set; }
        public int numero_clientes { get; set; }
        public int? duracionHoldSegundos { get; set; }
    }
}
