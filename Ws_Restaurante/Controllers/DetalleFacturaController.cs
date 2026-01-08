using GDatos.Entidades;
using Logica.Servicios;
using System;
using System.Data;
using System.Web.Http;

namespace Ws_GestionInterna.Controllers
{
    [RoutePrefix("api/detallefactura")]
    public class DetalleFacturaController : ApiController
    {
        private readonly DetalleFacturaLogica detalleLogica = new DetalleFacturaLogica();

        // ✅ GET: /api/detallefactura/{idFactura}
        [HttpGet]
        [Route("{idFactura:int}")]
        public IHttpActionResult ListarPorFactura(int idFactura)
        {
            try
            {
                DataTable dt = detalleLogica.ListarDetallesPorFactura(idFactura);

                if (dt == null || dt.Rows.Count == 0)
                    return Content(System.Net.HttpStatusCode.NotFound, new { mensaje = "No se encontraron detalles para la factura especificada." });

                return Ok(new
                {
                    mensaje = "Detalles obtenidos correctamente",
                    factura = idFactura,
                    detalles = dt
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ POST: /api/detallefactura/registrar
        [HttpPost]
        [Route("registrar")]
        public IHttpActionResult Registrar([FromBody] DetalleFactura nuevo)
        {
            try
            {
                if (nuevo == null)
                    return BadRequest("Debe enviar los datos del detalle.");

                detalleLogica.InsertarDetalle(nuevo);

                return Ok(new
                {
                    mensaje = "Detalle de factura registrado correctamente.",
                    subtotal = nuevo.Subtotal
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
