using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Ws_GIntegracionBus.DTOS
{
    public class DisponibilidadRequest
    {
        public string id_mesa { get; set; }
        public string fecha { get; set; }
        public int numeroPersonas { get; set; }
    }
}

