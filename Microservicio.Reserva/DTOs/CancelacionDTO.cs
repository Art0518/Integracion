namespace Microservicio.Reserva.DTOs
{
    public class CancelacionRequest
    {
        public string id_reserva { get; set; } = string.Empty;
    }

    public class CancelacionResponse
    {
        public bool exito { get; set; }
        public decimal valor_pagado { get; set; }
    }
}
