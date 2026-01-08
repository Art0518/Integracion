using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ws_GIntegracionBus.DTOS
{
    public class FacturaRequest
    {
        public int IdReserva { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public decimal Valor { get; set; }
    }

}
