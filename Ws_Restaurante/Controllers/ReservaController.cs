using System;
using System.Data;
using System.Web.Http;
using Logica.Servicios;
using GDatos.Entidades;
using System.Net;

namespace Ws_Restaurante.Controllers
{
    [RoutePrefix("api/reservas")]
    public class ReservaController : ApiController
    {
        private readonly ReservaLogica reservaLogica = new ReservaLogica();

        // ✅ GET: /api/reservas
        // Lista todas las reservas registradas
        [HttpGet]
        [Route("")]
        public IHttpActionResult ListarReservas()
        {
            try
            {
                DataTable reservas = reservaLogica.ListarReservas();
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ POST: /api/reservas/crear
        // Crea una nueva reserva
        [HttpPost]
        [Route("crear")]
        public IHttpActionResult CrearReserva([FromBody] Reserva reserva)
        {
            try
            {
                if (reserva == null)
                    return BadRequest("Los datos de la reserva son requeridos");

                DataTable resultado = reservaLogica.CrearReserva(reserva);

                // Verificar si hay un mensaje de error desde el stored procedure
                if (resultado.Rows.Count > 0 && resultado.Columns.Contains("Resultado"))
                {
                    string mensaje = resultado.Rows[0]["Resultado"].ToString();
                    if (mensaje.Contains("ya está reservada"))
                    {
                        // Retornar Conflict con contenido del mensaje
                        return Content(HttpStatusCode.Conflict, mensaje);
                    }
                    return Ok(new { mensaje = mensaje, datos = resultado });
                }

                return Ok(new { mensaje = "Reserva creada correctamente", datos = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ PUT: /api/reservas/estado
        // Cambia el estado de una reserva (CONFIRMADA, CANCELADA, FINALIZADA)
        [HttpPut]
        [Route("estado")]
        public IHttpActionResult ActualizarEstado([FromBody] ActualizarEstadoRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Los datos son requeridos");

                reservaLogica.ActualizarEstado(request.IdReserva, request.Estado);
                return Ok(new { mensaje = "Estado actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ GET: /api/reservas/{id}
        // Obtiene los detalles de una reserva por ID
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult ObtenerReservaPorId(int id)
        {
            try
            {
                DataTable reservas = reservaLogica.ListarReservas();
                DataRow[] fila = reservas.Select($"IdReserva = {id}");

                if (fila.Length == 0)
                    return NotFound();

                var r = fila[0];

                return Ok(new
                {
                    IdReserva = r["IdReserva"],
                    IdUsuario = r["IdUsuario"],
                    IdMesa = r["IdMesa"],
                    Fecha = r["Fecha"],
                    Hora = r["Hora"],
                    NumeroPersonas = r["NumeroPersonas"],
                    Estado = r["Estado"],
                    Observaciones = r["Observaciones"]
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ GET: /api/reservas/filtrar
        // Filtra reservas por usuario, estado o fecha
        [HttpGet]
        [Route("filtrar")]
        public IHttpActionResult FiltrarReservas(int? idUsuario = null, string estado = null, DateTime? fecha = null)
        {
            try
            {
                // Necesitarías agregar este método en tu ReservaLogica y ReservaDAO
                // Por ahora, filtramos del listado completo
                DataTable reservas = reservaLogica.ListarReservas();
                string filtro = "";

                if (idUsuario.HasValue)
                    filtro += $"IdUsuario = {idUsuario.Value}";

                if (!string.IsNullOrEmpty(estado))
                    filtro += (filtro != "" ? " AND " : "") + $"Estado = '{estado}'";

                if (fecha.HasValue)
                    filtro += (filtro != "" ? " AND " : "") + $"Fecha = '{fecha.Value:yyyy-MM-dd}'";

                DataRow[] filasFiltradas = string.IsNullOrEmpty(filtro) ?
                    reservas.Select() :
                    reservas.Select(filtro);

                DataTable resultado = reservas.Clone();
                foreach (DataRow row in filasFiltradas)
                {
                    resultado.ImportRow(row);
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    // Clase auxiliar para el request de actualizar estado
    public class ActualizarEstadoRequest
    {
        public int IdReserva { get; set; }
        public string Estado { get; set; }
    }
}