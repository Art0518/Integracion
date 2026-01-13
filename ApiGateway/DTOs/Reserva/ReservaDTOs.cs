using System.Text.Json.Serialization;

namespace ApiGateway.DTOs.Reserva
{
    public class PreReservaBusDTO
    {
  public string id_mesa { get; set; } = string.Empty;
        public DateTime fecha { get; set; }
        public int numero_clientes { get; set; }
        public int? duracionHoldSegundos { get; set; }
    }

    public class HoldResponse
    {
        [JsonPropertyName("idHold")]
        public string IdHold { get; set; } = string.Empty;

        [JsonPropertyName("idMesa")]
        public string IdMesa { get; set; } = string.Empty;
        
  [JsonPropertyName("fechaReserva")]
     public DateTime FechaReserva { get; set; }
        
        [JsonPropertyName("numeroPersonas")]
  public int NumeroPersonas { get; set; }
        
        [JsonPropertyName("duracionHoldSegundos")]
   public int DuracionHoldSegundos { get; set; }
        
        [JsonPropertyName("mensaje")]
      public string Mensaje { get; set; } = string.Empty;
    }

    public class ReservaBusDTO
    {
        public string id_mesa { get; set; } = string.Empty;
  public string id_hold { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
   public string apellido { get; set; } = string.Empty;
      public string correo { get; set; } = string.Empty;
public string tipo_identificacion { get; set; } = string.Empty;
   public string identificacion { get; set; } = string.Empty;
 public DateTime fecha { get; set; }
    public int personas { get; set; }
    }

    public class ConfirmarReservaResponse
    {
 public string Mensaje { get; set; } = string.Empty;
  public string IdReserva { get; set; } = string.Empty;
        public string IdMesa { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public string ApellidoCliente { get; set; } = string.Empty;
   public string Correo { get; set; } = string.Empty;
        public string TipoIdentificacion { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
  public string FechaReserva { get; set; } = string.Empty;
        public int NumeroPersonas { get; set; }
  public decimal ValorPagado { get; set; }
     public string UriFactura { get; set; } = string.Empty;
    }

    public class ReservaDetalleResponse
    {
   public string Mensaje { get; set; } = string.Empty;
        public string IdReserva { get; set; } = string.Empty;
 public string IdMesa { get; set; } = string.Empty;
public string NombreCliente { get; set; } = string.Empty;
  public string ApellidoCliente { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
public string TipoIdentificacion { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
   public string Fecha { get; set; } = string.Empty;
        public int NumeroPersonas { get; set; }
public decimal ValorPagado { get; set; }
        public string UriFactura { get; set; } = string.Empty;
    }

  public class CancelacionRequest
    {
        public string id_reserva { get; set; } = string.Empty;
    }

    public class CancelacionResponse
    {
        public bool exito { get; set; }
        public decimal valor_pagado { get; set; }
    }

    public class DisponibilidadRequest
    {
        public string id_mesa { get; set; } = string.Empty;
        public string fecha { get; set; } = string.Empty;
        public int numeroPersonas { get; set; }
    }

    public class DisponibilidadResponse
    {
   public int IdMesa { get; set; }
  public DateTime Fecha { get; set; }
        public bool Disponible { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
