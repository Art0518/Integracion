using GDatos.Entidades;
using Logica.Servicios;
using System;
using System.Data;
using System.Web.Http;

namespace Ws_GestionInterna.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [RoutePrefix("api/usuarios")]
    public class UsuarioController : ApiController
    {
        private readonly UsuarioLogica usuarioLogica = new UsuarioLogica();

        // ✅ POST: /api/usuarios/login
        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] Usuario datos)
        {
            try
            {
                DataTable result = usuarioLogica.Login(datos.Email, datos.Contrasena);

                if (result.Rows.Count == 0)
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { mensaje = "Credenciales inválidas" });

                var usuario = result.Rows[0];
                return Ok(new
                {
                    mensaje = "Login exitoso",
                    usuario = new
                    {
                        IdUsuario = usuario["IdUsuario"],

                        Email = usuario["Email"],


                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ POST: /api/usuarios/registrar
        [HttpPost]
        [Route("registrar")]
        public IHttpActionResult Registrar([FromBody] Usuario nuevo)
        {
            try
            {
                usuarioLogica.Registrar(nuevo);
                return Ok(new { mensaje = "Usuario registrado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ GET: /api/usuarios
        [HttpGet]
        [Route("")]
        public IHttpActionResult Listar(string rol = null, string estado = null)
        {
            try
            {
                DataTable dt = usuarioLogica.Listar(rol, estado);
                return Ok(dt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ PUT: /api/usuarios/{id}/estado
        [HttpPut]
        [Route("{id}/estado")]
        public IHttpActionResult CambiarEstado(int id, [FromBody] GDatos.Entidades.Usuario dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Estado))
                    return BadRequest("Debe especificar un estado.");

                usuarioLogica.CambiarEstado(id, dto.Estado);
                return Ok(new { mensaje = "Estado actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("{id}/actualizar")]
        public IHttpActionResult Actualizar(int id, [FromBody] Usuario u)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID de usuario no válido.");

                // Forzamos que el IdUsuario sea el de la URL
                u.IdUsuario = id;

                // Llamamos a la lógica
                usuarioLogica.Actualizar(u);

                return Ok(new { mensaje = "Usuario actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}