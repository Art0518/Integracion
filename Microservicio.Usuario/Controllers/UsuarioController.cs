using Microsoft.AspNetCore.Mvc;
using GDatos.Entidades;
using Logica.Servicios;
using Microservicio.Usuario.DTOs;
using System.Data;

namespace Microservicio.Usuario.Controllers
{
  [ApiController]
    [Route("api/usuarios")]
  public class UsuarioController : ControllerBase
    {
     private readonly UsuarioLogica _usuarioLogica = new UsuarioLogica();

   [HttpPost("registrar")]
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

      // Registrar el usuario y obtener el ID generado
   int usuarioId = _usuarioLogica.Registrar(usuario);

     // Construir la respuesta completa con todos los datos
            var response = new UsuarioCreadoResponseDTO
   {
  id = usuarioId,
         mensaje = "Usuario registrado correctamente.",
     nombre = body.nombre,
   apellido = body.apellido,
    email = body.email,
       tipo_identificacion = body.tipo_identificacion,
 identificacion = body.identificacion,
  _links = $"/api/usuarios/listar"
      };

return Ok(response);
            }
    catch (Exception ex)
    {
   return BadRequest("Error al registrar usuario: " + ex.Message);
        }
   }

     [HttpGet("listar")]
 [ProducesResponseType(typeof(UsuarioPaginadoResponseDTO), 200)]
   public IActionResult ListarUsuarios(
   [FromQuery] int pagina = 1,
      [FromQuery] int tamanoPagina = 50,
     [FromQuery] string? rol = null,
            [FromQuery] string? estado = null)
        {
   try
     {
      // Validaciones
    if (pagina < 1)
        pagina = 1;

           if (tamanoPagina < 1 || tamanoPagina > 100)
   tamanoPagina = 50; // Máximo 100 registros por página

   // Obtener datos paginados
      DataTable resultado = _usuarioLogica.ListarPaginado(pagina, tamanoPagina, rol, estado);
 var usuarios = new List<UsuarioListarResponseDTO>();
 
      int totalUsuarios = 0;
      bool foundMeta = false;

      foreach (DataRow row in resultado.Rows)
 {
        // La primera fila contiene metadatos
       if (Convert.ToInt32(row["IdUsuario"]) == -1 && !foundMeta)
            {
     totalUsuarios = Convert.ToInt32(row["Apellido"]);
   foundMeta = true;
           continue;
   }

 usuarios.Add(new UsuarioListarResponseDTO
    {
    idUsuario = Convert.ToInt32(row["IdUsuario"]),
nombre = row["Nombre"].ToString(),
 apellido = row["Apellido"].ToString(),
      email = row["Email"].ToString(),
      rol = row["Rol"].ToString()
   });
 }

     // Calcular información de paginación
       int totalPaginas = (int)Math.Ceiling((double)totalUsuarios / tamanoPagina);

     var response = new UsuarioPaginadoResponseDTO
            {
        usuarios = usuarios,
       totalUsuarios = totalUsuarios,
    paginaActual = pagina,
     totalPaginas = totalPaginas,
     tamanoPagina = tamanoPagina,
  tienePaginaAnterior = pagina > 1,
     tienePaginaSiguiente = pagina < totalPaginas,
     _links = GenerarLinksNavegacion(pagina, totalPaginas, tamanoPagina, rol, estado)
      };

   return Ok(response);
       }
       catch (Exception ex)
        {
  return BadRequest("Error al listar usuarios: " + ex.Message);
   }
  }

      private string GenerarLinksNavegacion(int pagina, int totalPaginas, int tamanoPagina, string? rol, string? estado)
     {
   var links = new List<string>();
    var baseUrl = "/api/usuarios/listar";
          var parametros = $"tamanoPagina={tamanoPagina}";

     if (!string.IsNullOrEmpty(rol))
  parametros += $"&rol={rol}";

    if (!string.IsNullOrEmpty(estado))
parametros += $"&estado={estado}";

            // Link primera página
      if (pagina > 1)
     links.Add($"primera: {baseUrl}?pagina=1&{parametros}");

     // Link página anterior
            if (pagina > 1)
   links.Add($"anterior: {baseUrl}?pagina={pagina - 1}&{parametros}");

       // Link página actual
links.Add($"actual: {baseUrl}?pagina={pagina}&{parametros}");

        // Link página siguiente
       if (pagina < totalPaginas)
       links.Add($"siguiente: {baseUrl}?pagina={pagina + 1}&{parametros}");

     // Link última página
      if (pagina < totalPaginas)
      links.Add($"ultima: {baseUrl}?pagina={totalPaginas}&{parametros}");

 return string.Join(", ", links);
      }
    }
}
