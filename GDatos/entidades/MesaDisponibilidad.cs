using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDatos.entidades
{
    public class MesaDisponibilidad
    {
        public bool Disponible { get; set; }
        public int MesasDisponibles { get; set; }
        public int CapacidadMaximaMesa { get; set; }
        public string HorarioRestaurante { get; set; }
    }
}

