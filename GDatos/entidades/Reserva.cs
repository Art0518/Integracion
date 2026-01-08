using System;

namespace GDatos.Entidades
{
    public class Reserva
    {
        public int IdReserva { get; set; }
        public int IdUsuario { get; set; }
        public int IdMesa { get; set; }
        public string TipoMesa { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public int NumeroPersonas { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
        public string MetodoPago { get; set; }
        public decimal? MontoDescuento { get; set; }
        public decimal? Total { get; set; }
        public string BookingId { get; set; }
    }
}
