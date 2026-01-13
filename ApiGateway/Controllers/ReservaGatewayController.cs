using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using ApiGateway.DTOs.Reserva;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/reservas")]
    [Tags("Reserva")]
    public class ReservaController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ReservaController> _logger;

        public ReservaController(IHttpClientFactory httpClientFactory, ILogger<ReservaController> logger)
   {
  _httpClientFactory = httpClientFactory;
          _logger = logger;
        }

   /// <summary>
   /// Crear una pre-reserva (hold)
        /// </summary>
  [HttpPost("hold")]
    [ProducesResponseType(typeof(HoldResponse), 200)]
     [ProducesResponseType(400)]
 public async Task<IActionResult> CrearPreReserva([FromBody] PreReservaBusDTO body)
   {
    try
   {
          var client = _httpClientFactory.CreateClient("ReservaService");
 var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
       var response = await client.PostAsync("api/reservas/hold", content);

      var responseBody = await response.Content.ReadAsStringAsync();
      
if (response.IsSuccessStatusCode)
    {
    var result = JsonSerializer.Deserialize<HoldResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
   return Ok(result);
  }
         
 return StatusCode((int)response.StatusCode, responseBody);
  }
     catch (Exception ex)
  {
            _logger.LogError(ex, "Error al crear pre-reserva");
   return BadRequest("Error al contactar el servicio de reservas: " + ex.Message);
     }
    }

  /// <summary>
        /// Confirmar una reserva
  /// </summary>
  [HttpPost("confirmar")]
[ProducesResponseType(typeof(ConfirmarReservaResponse), 200)]
        [ProducesResponseType(400)]
 public async Task<IActionResult> ConfirmarReserva([FromBody] ReservaBusDTO body)
     {
    try
    {
   var client = _httpClientFactory.CreateClient("ReservaService");
 var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
   var response = await client.PostAsync("api/reservas/confirmar", content);

         var responseBody = await response.Content.ReadAsStringAsync();
    
   if (response.IsSuccessStatusCode)
       {
   var result = JsonSerializer.Deserialize<ConfirmarReservaResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
 return Ok(result);
  }
    
  return StatusCode((int)response.StatusCode, responseBody);
     }
      catch (Exception ex)
       {
    _logger.LogError(ex, "Error al confirmar reserva");
      return BadRequest("Error al contactar el servicio de reservas: " + ex.Message);
 }
     }

   /// <summary>
     /// Buscar datos de una reserva por ID
        /// </summary>
   [HttpGet("{idReserva}")]
  [ProducesResponseType(typeof(ReservaDetalleResponse), 200)]
   [ProducesResponseType(404)]
public async Task<IActionResult> BuscarDatosReserva(string idReserva)
        {
try
   {
      var client = _httpClientFactory.CreateClient("ReservaService");
    var response = await client.GetAsync($"api/reservas/{idReserva}");

       var responseBody = await response.Content.ReadAsStringAsync();
   
   if (response.IsSuccessStatusCode)
    {
 var result = JsonSerializer.Deserialize<ReservaDetalleResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
      return Ok(result);
    }
      
   return StatusCode((int)response.StatusCode, responseBody);
      }
  catch (Exception ex)
            {
      _logger.LogError(ex, "Error al buscar reserva");
      return BadRequest("Error al contactar el servicio de reservas: " + ex.Message);
  }
        }

   /// <summary>
   /// Cancelar una reserva
 /// </summary>
        [HttpPut("cancelar/{idReserva}")]
    [ProducesResponseType(typeof(CancelacionResponse), 200)]
        public async Task<IActionResult> CancelarReserva(string idReserva)
    {
      try
            {
   var client = _httpClientFactory.CreateClient("ReservaService");
 var response = await client.PutAsync($"api/reservas/cancelar/{idReserva}", null);

var responseBody = await response.Content.ReadAsStringAsync();
     
     if (response.IsSuccessStatusCode)
    {
    var result = JsonSerializer.Deserialize<CancelacionResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
   return Ok(result);
  }
    
return StatusCode((int)response.StatusCode, responseBody);
      }
            catch (Exception ex)
 {
            _logger.LogError(ex, "Error al cancelar reserva");
    return BadRequest("Error al contactar el servicio de reservas: " + ex.Message);
            }
  }

  /// <summary>
    /// Validar disponibilidad de mesa
   /// </summary>
        [HttpPost("disponibilidad")]
 [ProducesResponseType(typeof(DisponibilidadResponse), 200)]
   [ProducesResponseType(400)]
   public async Task<IActionResult> ValidarDisponibilidad([FromBody] DisponibilidadRequest body)
     {
 try
  {
      var client = _httpClientFactory.CreateClient("ReservaService");
    var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
    var response = await client.PostAsync("api/reservas/disponibilidad", content);

       var responseBody = await response.Content.ReadAsStringAsync();
      
       if (response.IsSuccessStatusCode)
  {
       var result = JsonSerializer.Deserialize<DisponibilidadResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
       return Ok(result);
  }
    
return StatusCode((int)response.StatusCode, responseBody);
     }
         catch (Exception ex)
  {
   _logger.LogError(ex, "Error al validar disponibilidad");
   return BadRequest("Error al contactar el servicio de reservas: " + ex.Message);
      }
        }
    }
}
