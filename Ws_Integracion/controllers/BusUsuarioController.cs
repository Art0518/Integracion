using GDatos.Entidades;
using Logica.Servicios;
using System;
using System.Web.Http;
using System.Web.Http.Description;
using Ws_GIntegracionBus.DTOS;
using Ws_Integracion.dtos;

namespace Ws_GIntegracionBus.Controllers.V1
{
    [RoutePrefix("api/v1/integracion/restaurantes")]
    public class BusUsuarioController : ApiController
    {
        private readonly UsuarioLogica usuarioLogica = new UsuarioLogica();

        // 🟦 POST /api/v1/integracion/restaurantes/usuarios
        [HttpPost]
        [Route("usuarios")]
        [ResponseType(typeof(UsuarioCreadoResponseDTO))]
        public IHttpActionResult CrearUsuario([FromBody] UsuarioRequestDTO body)
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

                var usuario = new Usuario
                {
                    Nombre = body.nombre,
                    Apellido = body.apellido,
                    Email = body.email,
                    TipoIdentificacion = body.tipo_identificacion,
                    Identificacion = body.identificacion,
                    Rol = "CLIENTE",
                    Estado = "ACTIVO"
                };

                usuarioLogica.Registrar(usuario);

                // 🔥 Aquí devolvemos un JSON con un mensaje
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


        // 🟦 GET /api/v1/integracion/restaurantes/usuarios (queda pendiente si lo deseas)
    }
}
