using System.Text.Json.Serialization;

namespace ApiGateway.DTOs.Busqueda
{
    public class MesaResponse
    {
      [JsonPropertyName("idMesa")]
        public int IdMesa { get; set; }

  [JsonPropertyName("idRestaurante")]
        public int IdRestaurante { get; set; }

   [JsonPropertyName("numeroMesa")]
     public int NumeroMesa { get; set; }

        [JsonPropertyName("tipoMesa")]
   public string TipoMesa { get; set; } = string.Empty;

        [JsonPropertyName("capacidad")]
        public int Capacidad { get; set; }

        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }

        [JsonPropertyName("estado")]
   public string Estado { get; set; } = string.Empty;

        [JsonPropertyName("imagenURL")]
  public string ImagenURL { get; set; } = string.Empty;

    [JsonPropertyName("priceRange")]
        public object? PriceRange => null;
    }
}
