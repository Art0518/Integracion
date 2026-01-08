using GDatos.Entidades;
using Logica.Servicios;
using System;
using System.Data;
using System.Web.Http;

namespace Ws_Restaurante.Controllers
{
    [RoutePrefix("api/promociones")]
    public class PromocionController : ApiController
    {
        private readonly PromocionLogica promocionLogica = new PromocionLogica();

        // ✅ GET: /api/promociones
        [HttpGet]
        [Route("")]
        public IHttpActionResult Listar()
        {
            try
            {
                DataTable dt = promocionLogica.ListarPromociones();

                if (dt == null || dt.Rows.Count == 0)
                    return Content(System.Net.HttpStatusCode.NotFound, new { mensaje = "No existen promociones registradas." });

                return Ok(new
                {
                    mensaje = "Promociones obtenidas correctamente.",
                    promociones = dt
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Error al listar promociones: " + ex.Message);
            }
        }

        // ✅ POST: /api/promociones/gestionar
        [HttpPost]
        [Route("gestionar")]
        public IHttpActionResult Gestionar([FromBody] Promocion p)
        {
            try
            {
                if (p == null)
                    return BadRequest("Debe enviar los datos de la promoción.");

                promocionLogica.GestionarPromocion(p);

                string mensaje = p.IdPromocion > 0
                    ? "Promoción actualizada correctamente."
                    : "Promoción registrada correctamente.";

                return Ok(new { mensaje });
            }
            catch (Exception ex)
            {
                return BadRequest("Error al gestionar la promoción: " + ex.Message);
            }
        }
    }
}