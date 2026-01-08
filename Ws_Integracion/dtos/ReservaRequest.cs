using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ws_GIntegracionBus.DTOS
{
    public class ReservaRequest
    {
        public int IdUsuario { get; set; }
        public int IdMesa { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public int Personas { get; set; }
        public string Estado { get; set; }
    }
    public class ReservaBusDTO
    {
        public string id_mesa { get; set; }
        public string id_hold { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string correo { get; set; }
        public string tipo_identificacion { get; set; }
        public string identificacion { get; set; }
        public DateTime fecha { get; set; }
        public int personas { get; set; }
    }
    public class BuscarReservaDTO
    {
        public string id_reserva { get; set; }
    }
}
