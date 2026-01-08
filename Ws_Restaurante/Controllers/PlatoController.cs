using GDatos.Entidades;
using Logica.Servicios;
using System;
using System.Data;
using System.Web.Http;

namespace Ws_GestionInterna.Controllers
{
    [RoutePrefix("api/platos")]
    public class PlatoController : ApiController
    {
        private readonly PlatoLogica platoLogica = new PlatoLogica();

        // ✅ GET: /api/platos
        [HttpGet]
        [Route("")]
        public IHttpActionResult ListarPlatos()
        {
            try
            {
                DataTable platos = platoLogica.ListarPlatos();
                return Ok(platos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ POST: /api/platos/gestionar
        [HttpPost]
        [Route("gestionar")]
        public IHttpActionResult GestionarPlato([FromBody] Plato p)
        {
            try
            {
                platoLogica.GestionarPlato(p);
                string mensaje = p.IdPlato > 0 ? "Plato actualizado correctamente" : "Plato registrado correctamente";
                return Ok(new { mensaje });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ GET: /api/platos/{id}
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult ObtenerPlatoPorId(int id)
        {
            try
            {
                DataTable platos = platoLogica.ListarPlatos();
                DataRow[] fila = platos.Select($"IdPlato = {id}");

                if (fila.Length == 0)
                    return NotFound();

                var plato = fila[0];
                return Ok(new
                {
                    IdPlato = plato["IdPlato"],
                    Nombre = plato["Plato"],      // <- aquí cambiar a "Plato"
                    Precio = plato["Precio"],
                    Categoria = plato["Categoria"],
                    TipoComida = plato["TipoComida"],
                    Descripcion = plato["Descripcion"],
                    ImagenURL = plato["ImagenURL"]
                });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ DELETE: /api/platos/{id}
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult EliminarPlato(int id)
        {
            try
            {
                // En tu tabla no hay Estado, así que solo hacemos DELETE
                Plato p = new Plato
                {
                    IdPlato = id,
                    Operacion = "DELETE"
                };

                platoLogica.GestionarPlato(p);
                return Ok(new { mensaje = "Plato eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
