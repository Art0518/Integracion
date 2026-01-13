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

        /// <summary>
        /// Emite una nueva factura para una reserva específica.
        /// </summary>
        [HttpPost("emitir")]
        [Authorize(AuthenticationSchemes = "seguridad")]
        [ProducesResponseType(typeof(FacturaResponse), 200)]
        public IActionResult EmitirFactura([FromBody] FacturaRequest? body)
        {
            try
            {
                if (body == null)
                    return BadRequest("El cuerpo de la solicitud está vacío.");

                int idReserva = body.IdReserva;
                string correo = body.Email;
                string nombre = body.Nombre;
                string tipoIdentificacion = body.TipoIdentificacion;
                string identificacion = body.Identificacion;
                decimal valor = body.Valor;

                // Lógica para generar la factura
                DataTable dt = _facturaLogica.GenerarFactura(idReserva, correo, nombre, tipoIdentificacion, identificacion, valor);

                if (dt == null || dt.Rows.Count == 0)
                    return BadRequest("No se obtuvo respuesta al generar la factura.");

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

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al emitir factura: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtener los detalles de una factura por su ID.
        /// </summary>
        [HttpGet("invoices/{idFactura}")]
        [ProducesResponseType(200)]
        public IActionResult ObtenerFactura(int idFactura)
        {
            try
            {
                var factura = _facturaLogica.ObtenerFacturaPorId(idFactura);
                if (factura == null)
                {
                    return NotFound();
                }

                var detalles = _facturaLogica.ObtenerDetallesDeFactura(idFactura);

                // Transformar las filas de detalles
                var detallesTransformados = detalles.AsEnumerable().Select(d => new
                {
                    IdDetalle = d.Field<int>("IdDetalle"),
                    Descripcion = d.Field<string>("Descripcion"),
                    Cantidad = d.Field<int>("Cantidad"),
                    PrecioUnitario = d.Field<decimal>("PrecioUnitario"),
                    Subtotal = d.Field<decimal>("Subtotal"),
                    IdReserva = d.Field<int>("IdReserva"),
                    FechaReserva = d.Field<DateTime>("FechaReserva").Date,
                    Hora = d.Field<DateTime>("FechaReserva").ToString("HH:mm"),
                    NumeroPersonas = d.Field<int>("NumeroPersonas"),
                    NumeroMesa = d.Field<int>("NumeroMesa"),
                    TipoMesa = d.Field<string>("TipoMesa"),
                    CategoriaMesa = d.Field<string>("TipoMesa")
                }).ToList();

                var respuesta = new
                {
                    factura = new
                    {
                        id_factura = factura.Field<int>("IdFactura"),
                        fecha_hora = factura.Field<DateTime>("FechaHora").ToString("yyyy-MM-dd HH:mm:ss"),
                        subtotal = factura.Field<decimal>("Subtotal"),
                        iva = factura.Field<decimal>("IVA"),
                        total = factura.Field<decimal>("Total"),
                        estado = factura.Field<string>("EstadoFactura"),
                        cliente = new
                        {
                            nombre = factura.Field<string>("Cliente"),
                            email = factura.Field<string>("Email"),
                            cedula = factura.Field<string>("Cedula"),
                            tipo_identificacion = factura.Field<string>("TipoIdentificacion")
                        }
                    },
                    detalles = detallesTransformados,
                    uri_factura = $"http://ingtegracion-bar-sinson.runasp.net/api/v1/integracion/restaurantes/invoices/{idFactura}/document.pdf",
                    _links = new
                    {
                        verFactura = new
                        {
                            href = $"http://ingtegracion-bar-sinson.runasp.net/api/v1/integracion/restaurantes/invoices/{idFactura}",
                            method = "GET"
                        }
                    }
                };

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener la factura: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtener el PDF de una factura por su ID.
        /// </summary>
        [HttpGet("invoices/{idFactura}/pdf")]
        public IActionResult ObtenerPdfFactura(int idFactura)
        {
            try
            {
                // Construir la URI de la factura en PDF
                string baseUrl = $"{Request.Scheme}://{Request.Host}";
                var pdfUri = $"{baseUrl}/api/v1/integracion/restaurantes/invoices/{idFactura}/document.pdf";

                return Ok(new
                {
                    mensaje = "URI de factura disponible",
                    uri_factura = pdfUri,
                    id_factura = idFactura,
                    fecha_generacion = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffK"),
                    formato = "PDF",
                    _links = new
                    {
                        descargar = new
                        {
                            href = pdfUri,
                            method = "GET"
                        },
                        verFactura = new
                        {
                            href = $"{baseUrl}/api/v1/integracion/restaurantes/invoices/{idFactura}",
                            method = "GET"
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener el PDF de la factura: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene la factura por IdReserva.
        /// </summary>
        [HttpGet("{idReserva}")]
        [Authorize(AuthenticationSchemes = "seguridad")]
        [ProducesResponseType(typeof(FacturaGetResponse),200)]
        public IActionResult ObtenerFacturaPorReserva(string idReserva)
        {
            try
            {
                DataTable dt = _facturaLogica.ObtenerFacturaPorIdReserva(idReserva);
                if (dt == null || dt.Rows.Count ==0)
                    return NotFound();
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
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener la factura: " + ex.Message);
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
                // Buscar la factura por IdReserva
                DataTable dt = _facturaLogica.ObtenerFacturaPorIdReserva(idReserva);
                if (dt == null || dt.Rows.Count ==0)
                    return NotFound();
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
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener el PDF de la factura: " + ex.Message);
            }
        }
    }
}
