using System;

namespace Microservicio.Reserva.DTOs
{
    public class ReservaBusDTO
    {
        public string id_mesa { get; set; } = string.Empty;
        public string id_hold { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string apellido { get; set; } = string.Empty;
        public string correo { get; set; } = string.Empty;
        public string tipo_identificacion { get; set; } = string.Empty;
        public string identificacion { get; set; } = string.Empty;
        public DateTime fecha { get; set; }
        public int personas { get; set; }
    }
}
