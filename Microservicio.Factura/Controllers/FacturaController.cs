using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Logica.Servicios;
using Microservicio.Factura.DTOs;

namespace Microservicio.Factura.Controllers
{
    [ApiController]
    [Route("api/facturas")]
    public class FacturaController : ControllerBase
    {
        private readonly FacturaLogica _facturaLogica = new FacturaLogica();
        private readonly ILogger<FacturaController> _logger;

        public FacturaController(ILogger<FacturaController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Emite una nueva factura para una reserva específica.
        /// </summary>
        [HttpPost("emitir")]
        [ProducesResponseType(typeof(FacturaResponse), 200)]
        public IActionResult EmitirFactura([FromBody] FacturaRequest? body)
        {
            try
            {
                _logger.LogInformation("Iniciando emisión de factura");

                if (body == null)
                {
                    _logger.LogWarning("El cuerpo de la solicitud está vacío");
                    return BadRequest("El cuerpo de la solicitud está vacío.");
                }

                int idReserva = body.IdReserva;
                string correo = body.Email;
                string nombre = body.Nombre;
                string tipoIdentificacion = body.TipoIdentificacion;
                string identificacion = body.Identificacion;
                decimal valor = body.Valor;

                _logger.LogInformation($"Generando factura para reserva {idReserva}");

                // Lógica para generar la factura
                DataTable dt = _facturaLogica.GenerarFactura(idReserva, correo, nombre, tipoIdentificacion, identificacion, valor);

                if (dt == null || dt.Rows.Count == 0)
                {
                    _logger.LogWarning($"No se obtuvo respuesta al generar la factura para reserva {idReserva}");
                    return BadRequest("No se obtuvo respuesta al generar la factura.");
                }

                var row = dt.Rows[0];

                var response = new FacturaResponse
                {
                    IdFactura = row.Table.Columns.Contains("IdFactura") && row["IdFactura"] != DBNull.Value ? Convert.ToInt32(row["IdFactura"]) : 0,
                    IdReserva = row.Table.Columns.Contains("IdReserva") && row["IdReserva"] != DBNull.Value ? row["IdReserva"].ToString() ?? string.Empty : idReserva.ToString(),
                    Correo = row.Table.Columns.Contains("Correo") && row["Correo"] != DBNull.Value ? row["Correo"].ToString() ?? string.Empty : correo,
                    Nombre = row.Table.Columns.Contains("Nombre") && row["Nombre"] != DBNull.Value ? row["Nombre"].ToString() ?? string.Empty : nombre,
                    TipoIdentificacion = row.Table.Columns.Contains("TipoIdentificacion") && row["TipoIdentificacion"] != DBNull.Value ? row["TipoIdentificacion"].ToString() ?? string.Empty : tipoIdentificacion,
                    Identificacion = row.Table.Columns.Contains("Identificacion") && row["Identificacion"] != DBNull.Value ? row["Identificacion"].ToString() ?? string.Empty : identificacion,
                    Subtotal = row.Table.Columns.Contains("Subtotal") && row["Subtotal"] != DBNull.Value ? Convert.ToDecimal(row["Subtotal"]) : 0,
                    Iva = row.Table.Columns.Contains("IVA") && row["IVA"] != DBNull.Value ? Convert.ToDecimal(row["IVA"]) : 0,
                    Total = row.Table.Columns.Contains("Total") && row["Total"] != DBNull.Value ? Convert.ToDecimal(row["Total"]) : 0,
                    EstadoFactura = row.Table.Columns.Contains("EstadoFactura") && row["EstadoFactura"] != DBNull.Value ? row["EstadoFactura"].ToString() ?? string.Empty : string.Empty,
                    EstadoReserva = row.Table.Columns.Contains("EstadoReserva") && row["EstadoReserva"] != DBNull.Value ? row["EstadoReserva"].ToString() ?? string.Empty : string.Empty,
                    FechaGeneracion = row.Table.Columns.Contains("FechaGeneracion") && row["FechaGeneracion"] != DBNull.Value ? Convert.ToDateTime(row["FechaGeneracion"]) : DateTime.Now,
                    UriFactura = $"/api/facturas/{(row.Table.Columns.Contains("IdFactura") && row["IdFactura"] != DBNull.Value ? row["IdFactura"].ToString() : "0")}/pdf"
                };

                _logger.LogInformation($"Factura generada exitosamente: {response.IdFactura}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al emitir factura");
                return StatusCode(500, new { error = "Error al emitir factura", message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        /// <summary>
        /// Obtiene la factura por IdReserva.
        /// </summary>
        [HttpGet("{idReserva}")]
        [ProducesResponseType(typeof(FacturaGetResponse),200)]
        public IActionResult ObtenerFacturaPorReserva(string idReserva)
        {
            try
            {
                _logger.LogInformation($"Obteniendo factura para reserva {idReserva}");

                DataTable dt = _facturaLogica.ObtenerFacturaPorIdReserva(idReserva);
                if (dt == null || dt.Rows.Count ==0)
                {
                    _logger.LogWarning($"No se encontró factura para reserva {idReserva}");
                    return NotFound(new { error = "Factura no encontrada", idReserva });
                }

                var row = dt.Rows[0];
                var idFactura = row.Table.Columns.Contains("IdFactura") && row["IdFactura"] != DBNull.Value ? row["IdFactura"].ToString() : "0";
                var response = new
                {
                    idFactura = row.Table.Columns.Contains("IdFactura") && row["IdFactura"] != DBNull.Value ? Convert.ToInt32(row["IdFactura"]) :0,
                    idReserva = idReserva,
                    subtotal = row.Table.Columns.Contains("Subtotal") && row["Subtotal"] != DBNull.Value ? Convert.ToDecimal(row["Subtotal"]) :0,
                    iva = row.Table.Columns.Contains("IVA") && row["IVA"] != DBNull.Value ? Convert.ToDecimal(row["IVA"]) :0,
                    total = row.Table.Columns.Contains("Total") && row["Total"] != DBNull.Value ? Convert.ToDecimal(row["Total"]) :0,
                    estadoFactura = row.Table.Columns.Contains("EstadoFactura") && row["EstadoFactura"] != DBNull.Value ? row["EstadoFactura"].ToString() ?? string.Empty : string.Empty,
                    fechaGeneracion = row.Table.Columns.Contains("FechaGeneracion") && row["FechaGeneracion"] != DBNull.Value ? Convert.ToDateTime(row["FechaGeneracion"]) : DateTime.Now,
                    uri_factura = $"https://factura-production-7d28.up.railway.app/api/facturas/{idReserva}/document.pdf",
                    _links = new
                    {
                        verFactura = new
                        {
                            href = $"https://factura-production-7d28.up.railway.app/api/facturas/{idReserva}",
                            method = "GET"
                        }
                    }
                };

                _logger.LogInformation($"Factura encontrada: {response.idFactura}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la factura para reserva {idReserva}");
                return StatusCode(500, new { error = "Error al obtener la factura", message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        /// <summary>
        /// Obtener el PDF de una factura por IdReserva.
        /// </summary>
        [HttpGet("{idReserva}/pdf")]
        public IActionResult ObtenerPdfFacturaPorReserva(string idReserva)
        {
            try
            {
                _logger.LogInformation($"Obteniendo PDF para reserva {idReserva}");

                // Buscar la factura por IdReserva
                DataTable dt = _facturaLogica.ObtenerFacturaPorIdReserva(idReserva);
                if (dt == null || dt.Rows.Count ==0)
                {
                    _logger.LogWarning($"No se encontró factura para generar PDF de reserva {idReserva}");
                    return NotFound(new { error = "Factura no encontrada", idReserva });
                }

                var row = dt.Rows[0];
                var idFactura = row.Table.Columns.Contains("IdFactura") && row["IdFactura"] != DBNull.Value ? Convert.ToInt32(row["IdFactura"]) :0;
                var fechaGeneracion = row.Table.Columns.Contains("FechaGeneracion") && row["FechaGeneracion"] != DBNull.Value ? Convert.ToDateTime(row["FechaGeneracion"]).ToString("yyyy-MM-ddTHH:mm:ss.fffK") : DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffK");
                var uri = $"https://factura-production-7d28.up.railway.app/api/facturas/{idReserva}/document.pdf";
                var response = new
                {
                    mensaje = "URI de factura disponible",
                    uri_factura = uri,
                    id_factura = idFactura,
                    fecha_generacion = fechaGeneracion,
                    formato = "PDF",
                    _links = new
                    {
                        descargar = new
                        {
                            href = uri,
                            method = "GET"
                        },
                        verFactura = new
                        {
                            href = $"https://factura-production-7d28.up.railway.app/api/facturas/{idReserva}",
                            method = "GET"
                        }
                    }
                };

                _logger.LogInformation($"PDF disponible para factura {idFactura}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el PDF de la factura para reserva {idReserva}");
                return StatusCode(500, new { error = "Error al obtener el PDF de la factura", message = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}
