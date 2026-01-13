using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using ApiGateway.DTOs.Factura;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/facturas")]
    [Tags("Factura")]
    public class FacturaController : ControllerBase
    {
      private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<FacturaController> _logger;

   public FacturaController(IHttpClientFactory httpClientFactory, ILogger<FacturaController> logger)
    {
  _httpClientFactory = httpClientFactory;
 _logger = logger;
        }

    /// <summary>
 /// Emitir una nueva factura
        /// </summary>
  [HttpPost("emitir")]
   [ProducesResponseType(typeof(FacturaResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> EmitirFactura([FromBody] FacturaRequest body)
   {
      try
  {
     var client = _httpClientFactory.CreateClient("FacturaService");
         var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
      var response = await client.PostAsync("api/facturas/emitir", content);

       var responseBody = await response.Content.ReadAsStringAsync();
        
  if (response.IsSuccessStatusCode)
       {
       var result = JsonSerializer.Deserialize<FacturaResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
          return Ok(result);
   }
       
   return StatusCode((int)response.StatusCode, responseBody);
     }
catch (Exception ex)
   {
        _logger.LogError(ex, "Error al emitir factura");
   return BadRequest("Error al contactar el servicio de facturas: " + ex.Message);
       }
     }

   /// <summary>
      /// Obtener factura por ID de reserva
 /// </summary>
   [HttpGet("{idReserva}")]
        [ProducesResponseType(typeof(FacturaGetResponse), 200)]
        [ProducesResponseType(404)]
 public async Task<IActionResult> ObtenerFacturaPorReserva(string idReserva)
   {
      try
    {
 var client = _httpClientFactory.CreateClient("FacturaService");
      var response = await client.GetAsync($"api/facturas/{idReserva}");

  var responseBody = await response.Content.ReadAsStringAsync();
          
    if (response.IsSuccessStatusCode)
          {
        var result = JsonSerializer.Deserialize<object>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return Ok(result);
  }
         
  return StatusCode((int)response.StatusCode, responseBody);
    }
  catch (Exception ex)
   {
     _logger.LogError(ex, "Error al obtener factura");
return BadRequest("Error al contactar el servicio de facturas: " + ex.Message);
   }
   }

      /// <summary>
        /// Obtener PDF de factura
      /// </summary>
        [HttpGet("{idReserva}/pdf")]
        [ProducesResponseType(200)]
   public async Task<IActionResult> ObtenerPdfFactura(string idReserva)
  {
     try
      {
           var client = _httpClientFactory.CreateClient("FacturaService");
        var response = await client.GetAsync($"api/facturas/{idReserva}/pdf");

var responseBody = await response.Content.ReadAsStringAsync();
      
            if (response.IsSuccessStatusCode)
  {
     var result = JsonSerializer.Deserialize<object>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    return Ok(result);
  }
      
     return StatusCode((int)response.StatusCode, responseBody);
     }
    catch (Exception ex)
            {
         _logger.LogError(ex, "Error al obtener PDF de factura");
    return BadRequest("Error al contactar el servicio de facturas: " + ex.Message);
       }
    }
 }
}
