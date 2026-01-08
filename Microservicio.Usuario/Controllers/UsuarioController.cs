using Microsoft.AspNetCore.Mvc;
using GDatos.Entidades;
using Logica.Servicios;
using Microservicio.Usuario.DTOs;

namespace Microservicio.Usuario.Controllers
{
    [ApiController]
    [Route("api/v1/integracion/restaurantes")]
    public class UsuarioController : ControllerBase
    {
 private readonly UsuarioLogica _usuarioLogica = new UsuarioLogica();

        [HttpPost("usuarios")]
        [ProducesResponseType(typeof(UsuarioCreadoResponseDTO), 200)]
      public IActionResult CrearUsuario([FromBody] UsuarioRequestDTO? body)
   {
    try
   {
            if (body == null)
      return BadRequest("El cuerpo de la solicitud está vacío.");

   if (string.IsNullOrWhiteSpace(body.nombre) ||
        string.IsNullOrWhiteSpace(body.apellido) ||
      string.IsNullOrWhiteSpace(body.email) ||
  string.IsNullOrWhiteSpace(body.tipo_identificacion) ||
     string.IsNullOrWhiteSpace(body.identificacion))
   {
       return BadRequest("Todos los campos son obligatorios: nombre, apellido, email, tipo_identificación, identificación.");
      }

 var usuario = new GDatos.Entidades.Usuario
       {
  Nombre = body.nombre,
    Apellido = body.apellido,
      Email = body.email,
TipoIdentificacion = body.tipo_identificacion,
     Identificacion = body.identificacion,
        Rol = "CLIENTE",
       Estado = "ACTIVO"
         };

         _usuarioLogica.Registrar(usuario);

    return Ok(new UsuarioCreadoResponseDTO
  {
      mensaje = "Usuario registrado correctamente."
     });
     }
            catch (Exception ex)
{
  return BadRequest("Error al registrar usuario: " + ex.Message);
 }
    }
    }
}
