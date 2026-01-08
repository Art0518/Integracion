using System;
using System.Web.Http;
using Logica.Servicios;
using GDatos.Entidades;
using System.Data;

namespace Ws_Restaurante.Controllers
{
    // 🔹 Prefijo base de la API
    [RoutePrefix("api/facturas")]
    public class FacturaController : ApiController
    {
        private readonly FacturaLogica facturaLogica = new FacturaLogica();

        // ✅ GET /api/facturas
        // Devuelve todas las facturas registradas
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetFacturas()
        {
            try
            {
                DataTable dt = facturaLogica.ListarFacturas();
                return Ok(dt);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al listar facturas: " + ex.Message));
            }
        }

        // ✅ GET /api/facturas/{id}
        // Devuelve el detalle de una factura específica
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetFactura(int id)
        {
            try
            {
                DataTable dt = facturaLogica.DetalleFactura(id);
                if (dt.Rows.Count == 0)
                    return NotFound();

                return Ok(dt);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al obtener la factura: " + ex.Message));
            }
        }

        // ✅ POST /api/facturas
        // Crea una nueva factura a partir de una reserva
        [HttpPost]
        [Route("")]
        public IHttpActionResult CrearFactura([FromBody] dynamic body)
        {
            try
            {
                int idUsuario = (int)body.idUsuario;
                int idReserva = (int)body.idReserva;

                //DataTable dt = facturaLogica.GenerarFactura(idUsuario, idReserva);
                
                return Ok(new
                {
                    mensaje = "Factura generada correctamente",
                   
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Error al generar la factura: " + ex.Message);
            }
        }

        // ✅ PUT /api/facturas/{id}/anular
        // Anula una factura existente
        [HttpPut]
        [Route("{id:int}/anular")]
        public IHttpActionResult AnularFactura(int id)
        {
            try
            {
                facturaLogica.AnularFactura(id);
                return Ok(new { mensaje = "Factura anulada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest("Error al anular la factura: " + ex.Message);
            }
        }

        // ✅ POST /api/facturas/calcular
        // Calcula el subtotal, IVA y total sin guardar
        [HttpPost]
        [Route("calcular")]
        public IHttpActionResult CalcularTotales([FromBody] Factura factura)
        {
            try
            {
                Factura f = facturaLogica.CalcularTotales(factura);
                return Ok(f);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al calcular totales: " + ex.Message);
            }
        }
    }
}