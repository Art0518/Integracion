using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ws_GIntegracionBus.DTOS
{
    public class UsuarioRequestDTO
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string tipo_identificacion { get; set; }
        public string identificacion { get; set; }
    }
}

