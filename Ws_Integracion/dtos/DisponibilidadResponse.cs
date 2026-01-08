using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace Ws_GIntegracionBus.DTOS
{
    public class DisponibilidadResponse
    {
        public int IdMesa { get; set; }
        public System.DateTime Fecha { get; set; }
        public bool Disponible { get; set; }
        public string Mensaje { get; set; }
    }
}

