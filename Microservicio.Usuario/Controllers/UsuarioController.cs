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
        [ProducesResponseType(typeof(List<UsuarioListarResponseDTO>), 200)]
        public IActionResult ListarUsuarios()
        {
            try
            {
                DataTable dt = _usuarioLogica.Listar();
                var usuarios = new List<UsuarioListarResponseDTO>();

                foreach (DataRow row in dt.Rows)
                {
                    usuarios.Add(new UsuarioListarResponseDTO
                    {
                        idUsuario = Convert.ToInt32(row["IdUsuario"]),
                        nombre = row["Nombre"].ToString(),
                        apellido = row["Apellido"].ToString(),
                        email = row["Email"].ToString(),
                        rol = row["Rol"].ToString()
                    });
                }

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al listar usuarios: " + ex.Message);
            }
        }
    }
}
