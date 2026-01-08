using System;

namespace Ws_GIntegracionBus.Dtos
{
    public class PreReservaDTO
    {
        public DateTime fecha { get; set; }
        public string hora { get; set; }
        public int personas { get; set; }
        public int bookingUserId { get; set; }
        public int idMesa { get; set; }
        public int? duracionHoldSegundos { get; set; }
    }

    public class PreReservaBusDTO
    {
        public string id_mesa { get; set; }
        public DateTime fecha { get; set; }
        public int numero_clientes { get; set; }
        public int? duracionHoldSegundos { get; set; }
    }
}
