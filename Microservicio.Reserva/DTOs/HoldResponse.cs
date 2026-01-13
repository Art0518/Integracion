using System.Text.Json.Serialization;

namespace Microservicio.Reserva.DTOs
{
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
}