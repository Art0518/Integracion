using GDatos.entidades; // <-- Importante para usar MesaDisponibilidad
using Logica.Servicios;
using System;
using System.Web.Http;
using System.Web.Http.Description;
using Ws_GIntegracionBus.DTOS;

namespace Ws_GIntegracionBus.Controllers.V1
{
    [RoutePrefix("api/v1/integracion/restaurantes")]
    public class BusDisponibilidadController : ApiController
    {
        private readonly MesaLogica mesaLogica = new MesaLogica();

        /// <summary>
        /// Valida la disponibilidad de una mesa específica para una fecha y número de personas.
        /// </summary>
        [HttpPost]
        [Route("availability")]
        [ResponseType(typeof(DisponibilidadResponse))]
        public IHttpActionResult ValidarDisponibilidad([FromBody] DisponibilidadRequest body)
        {
            try
            {
                if (body == null)
                    return BadRequest("El cuerpo de la solicitud está vacío.");

                DateTime fecha;
                if (!DateTime.TryParse(body.fecha, out fecha))
                    return BadRequest("Fecha inválida.");

                var disponibilidad = mesaLogica.ConsultarDisponibilidad(body.id_mesa, fecha, body.numeroPersonas, "San Juan");

                int idMesaResp = 0;
                int.TryParse(body.id_mesa, out idMesaResp);

                var response = new DisponibilidadResponse
                {
                    IdMesa = idMesaResp,
                    Fecha = fecha,
                    Disponible = disponibilidad.Disponible,
                    Mensaje = disponibilidad.Disponible ? "Mesa disponible." : "Mesa no disponible."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al validar disponibilidad: " + ex.Message);
            }
        }
    }
}
