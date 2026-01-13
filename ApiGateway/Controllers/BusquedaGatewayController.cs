using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ApiGateway.DTOs.Busqueda;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/mesas")]
    [Tags("Busqueda")]
    public class BusquedaController : ControllerBase
    {
      private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BusquedaController> _logger;

        public BusquedaController(IHttpClientFactory httpClientFactory, ILogger<BusquedaController> logger)
 {
  _httpClientFactory = httpClientFactory;
       _logger = logger;
  }

        /// <summary>
        /// Buscar mesas disponibles
      /// </summary>
  [HttpGet("buscar")]
        [ProducesResponseType(typeof(List<MesaResponse>), 200)]
      public async Task<IActionResult> BuscarMesas(
    [FromQuery] int? capacidad = null,
 [FromQuery] string? tipoMesa = null,
     [FromQuery] string? estado = null)
   {
      try
     {
      var client = _httpClientFactory.CreateClient("BusquedaService");
       var queryParams = new List<string>();
  
   if (capacidad.HasValue)
      queryParams.Add($"capacidad={capacidad}");
if (!string.IsNullOrEmpty(tipoMesa))
 queryParams.Add($"tipoMesa={tipoMesa}");
       if (!string.IsNullOrEmpty(estado))
        queryParams.Add($"estado={estado}");

  var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
       var response = await client.GetAsync($"api/mesas/buscar{query}");

 var responseBody = await response.Content.ReadAsStringAsync();
    
    if (response.IsSuccessStatusCode)
            {
    var result = JsonSerializer.Deserialize<List<MesaResponse>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
       return Ok(result);
      }
        
 return StatusCode((int)response.StatusCode, responseBody);
       }
     catch (Exception ex)
 {
    _logger.LogError(ex, "Error al buscar mesas");
   return BadRequest("Error al contactar el servicio de búsqueda: " + ex.Message);
       }
        }
    }
}
