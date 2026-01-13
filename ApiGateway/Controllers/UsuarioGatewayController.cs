using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using ApiGateway.DTOs.Usuario;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    [Tags("Usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IHttpClientFactory httpClientFactory, ILogger<UsuarioController> logger)
        {
     _httpClientFactory = httpClientFactory;
         _logger = logger;
     }

        /// <summary>
        /// Registrar un nuevo usuario
    /// </summary>
        [HttpPost("registrar")]
        [ProducesResponseType(typeof(UsuarioCreadoResponseDTO), 200)]
        [ProducesResponseType(400)]
   public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioRequestDTO body)
        {
            try
            {
         var client = _httpClientFactory.CreateClient("UsuarioService");
     var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/usuarios/registrar", content);

        var responseBody = await response.Content.ReadAsStringAsync();
    
    if (response.IsSuccessStatusCode)
       {
        var result = JsonSerializer.Deserialize<UsuarioCreadoResponseDTO>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
             return Ok(result);
    }
           
      return StatusCode((int)response.StatusCode, responseBody);
       }
     catch (Exception ex)
   {
     _logger.LogError(ex, "Error al registrar usuario");
    return BadRequest("Error al contactar el servicio de usuarios: " + ex.Message);
   }
      }

    /// <summary>
     /// Listar usuarios con paginación
        /// </summary>
        [HttpGet("listar")]
        [ProducesResponseType(typeof(UsuarioPaginadoResponseDTO), 200)]
        public async Task<IActionResult> ListarUsuarios(
   [FromQuery] int pagina = 1,
      [FromQuery] int tamanoPagina = 50,
      [FromQuery] string? rol = null,
       [FromQuery] string? estado = null)
        {
      try
            {
         var client = _httpClientFactory.CreateClient("UsuarioService");
      var queryParams = $"?pagina={pagina}&tamanoPagina={tamanoPagina}";
                
       if (!string.IsNullOrEmpty(rol))
      queryParams += $"&rol={rol}";
             if (!string.IsNullOrEmpty(estado))
       queryParams += $"&estado={estado}";

    var response = await client.GetAsync($"api/usuarios/listar{queryParams}");
        var responseBody = await response.Content.ReadAsStringAsync();
      
          if (response.IsSuccessStatusCode)
     {
          var result = JsonSerializer.Deserialize<UsuarioPaginadoResponseDTO>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
      return Ok(result);
  }
  
      return StatusCode((int)response.StatusCode, responseBody);
   }
         catch (Exception ex)
  {
     _logger.LogError(ex, "Error al listar usuarios");
    return BadRequest("Error al contactar el servicio de usuarios: " + ex.Message);
     }
        }
    }
}
