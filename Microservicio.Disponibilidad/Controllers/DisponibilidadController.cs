using Microsoft.AspNetCore.Mvc;
using Logica.Servicios;
using Microservicio.Disponibilidad.DTOs;

namespace Microservicio.Disponibilidad.Controllers
{
    [ApiController]
    [Route("api/v1/integracion/restaurantes")]
 public class DisponibilidadController : ControllerBase
    {
        private readonly MesaLogica _mesaLogica = new MesaLogica();

        /// <summary>
     /// Valida la disponibilidad de una mesa específica para una fecha y número de personas.
        /// </summary>
     [HttpPost("availability")]
        [ProducesResponseType(typeof(DisponibilidadResponse), 200)]
    public IActionResult ValidarDisponibilidad([FromBody] DisponibilidadRequest? body)
  {
try
  {
  if (body == null)
       return BadRequest("El cuerpo de la solicitud está vacío.");

 DateTime fecha;
          if (!DateTime.TryParse(body.fecha, out fecha))
  return BadRequest("Fecha inválida.");

 var disponibilidad = _mesaLogica.ConsultarDisponibilidad(body.id_mesa, fecha, body.numeroPersonas, "San Juan");

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
