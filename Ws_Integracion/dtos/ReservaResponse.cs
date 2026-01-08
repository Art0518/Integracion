using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ws_GIntegracionBus.DTOS
{
    public class ReservaResponse
    {
        public string Mensaje { get; set; }
        public int IdReserva { get; set; }
        public int IdMesa { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Estado { get; set; }
    }
    public class PreReservaBusResponse
    {
        public string IdMesa { get; set; }
        public DateTime FechaReserva { get; set; }
        public int NumeroPersonas { get; set; }
        public int DuracionHoldSegundos { get; set; }
        public string IdHold { get; set; }
        public string Mensaje { get; set; }
    }
    public class ReservaBusResponse
    {
        public string idReserva { get; set; }
        public decimal valor_pagado { get; set; }
        public string uri_factura { get; set; }
        public string mensaje { get; set; }
    }
    public class BuscarReservaResponse
    {
        public string numero_mesa { get; set; }
        public string correo { get; set; }
        public string fecha { get; set; }
        public int numero_personas { get; set; }
        public string categoria { get; set; }
        public string tipo { get; set; }
        public int capacidad { get; set; }
        public string nombre { get; set; }
        public decimal valor_pagado { get; set; }
        public string uri_factura { get; set; }
    }
    public class ReservaConfirmacionResponse
    {
        public string Mensaje { get; set; }
        public string IdReserva { get; set; }
        public string Estado { get; set; }
        public decimal ValorPagado { get; set; }
        public string UriFactura { get; set; }
        public string NombreCliente { get; set; }
        public string CorreoCliente { get; set; }
        public DateTime FechaReserva { get; set; }
        public string NumeroMesa { get; set; }
    }
    public class CancelacionRequest
    {
        public string id_reserva { get; set; }
    }

    public class CancelacionResponse
    {
        public bool exito { get; set; }
        public decimal valor_pagado { get; set; }
    }
    public class ConfirmarReservaResponse
    {
        public string Mensaje { get; set; }
        public string IdReserva { get; set; }
        public string IdMesa { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string Correo { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string FechaReserva { get; set; }
        public int NumeroPersonas { get; set; }
        public decimal ValorPagado { get; set; }
        public string UriFactura { get; set; }
    }
    public class ReservaDetalleResponse
    {
        public string Mensaje { get; set; }
        public string IdReserva { get; set; }
        public string IdMesa { get; set; }
        public string Fecha { get; set; }
        public int NumeroPersonas { get; set; }
        public string Estado { get; set; }
        public string TipoMesa { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string Correo { get; set; }
        public decimal ValorPagado { get; set; }
        public string UriFactura { get; set; }
    }
}
