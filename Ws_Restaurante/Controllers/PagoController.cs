using System;
using System.Web.Http;
using Logica.Servicios;
using GDatos.Entidades;
using System.Data;

namespace Ws_Restaurante.Controllers
{
    [RoutePrefix("api/pagos")]
    public class PagoController : ApiController
    {
        private readonly PagoLogica pagoLogica = new PagoLogica();

        // ✅ POST /api/pagos
        // Registrar un nuevo pago
        [HttpPost]
        [Route("")]
        public IHttpActionResult RegistrarPago([FromBody] Pago pago)
        {
            try
            {
                pago.FechaPago = DateTime.Now;
                pago.Estado = "COMPLETADO";

                pagoLogica.RegistrarPago(pago);

                return Ok(new
                {
                    mensaje = "Pago registrado correctamente.",
                    pago
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Error al registrar pago: " + ex.Message);
            }
        }
        // ✅ GET /api/pagos/{id}
        // Obtener un pago específico
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult ObtenerPago(int id)
        {
            try
            {
                DataTable dt = pagoLogica.ValidarPago(id);

                if (dt.Rows.Count == 0)
                    return NotFound(); // No se encontró el pago

                return Ok(dt);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener pago: " + ex.Message);
            }
        }


        // ✅ GET /api/pagos/{id}
        // Validar un pago existente
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult ValidarPago(int id)
        {
            try
            {
                DataTable dt = pagoLogica.ValidarPago(id);

                if (dt.Rows.Count == 0)
                    return NotFound();

                return Ok(dt);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al validar pago: " + ex.Message);
            }
        }
    }
}