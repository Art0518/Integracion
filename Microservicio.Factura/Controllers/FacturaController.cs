using Microsoft.AspNetCore.Mvc;
using System.Data;
using Logica.Servicios;
using Microservicio.Factura.DTOs;

namespace Microservicio.Factura.Controllers
{
    [ApiController]
    [Route("api/v1/integracion/restaurantes")]
    public class FacturaController : ControllerBase
    {
        private readonly FacturaLogica _facturaLogica = new FacturaLogica();

        /// <summary>
        /// Genera una factura para una reserva específica.
        /// </summary>
        [HttpPost("invoices")]
        [ProducesResponseType(typeof(FacturaResponse), 200)]
        public IActionResult EmitirFactura([FromBody] FacturaRequest? body)
        {
            try
            {
                if (body == null)
                    return BadRequest("El cuerpo de la solicitud está vacío.");

                int idReserva = Convert.ToInt32(body.IdReserva);
                string correo = body.Email;
                string nombre = body.Nombre;
                string tipoIdentificacion = body.TipoIdentificacion;
                string identificacion = body.Identificacion;
                decimal valor = body.Valor;

                // Llamada a la lógica para generar la factura
                DataTable dt = _facturaLogica.GenerarFactura(idReserva, correo, nombre, tipoIdentificacion, identificacion, valor);

                if (dt == null || dt.Rows.Count == 0)
                    return BadRequest("No se obtuvo respuesta al generar la factura.");

                var row = dt.Rows[0];

                // Obtener IdFactura
                int idFactura = row.Table.Columns.Contains("IdFactura") && row["IdFactura"] != DBNull.Value
                    ? Convert.ToInt32(row["IdFactura"])
                    : 0;

                // Mensaje original del SP
                string mensajeOriginal = row.Table.Columns.Contains("Mensaje") ? row["Mensaje"]?.ToString() ?? "" : "";

                // Crear mensaje que incluya el ID de factura
                string mensajeFinal = !string.IsNullOrEmpty(mensajeOriginal)
                    ? $"{mensajeOriginal} ID de factura: {idFactura}"
                    : $"Factura emitida correctamente. ID de factura: {idFactura}";

                var response = new FacturaResponse
                {
                    Mensaje = mensajeFinal,
                    IdFactura = idFactura,
                    IdUsuario = row.Table.Columns.Contains("IdUsuario") && row["IdUsuario"] != DBNull.Value ? Convert.ToInt32(row["IdUsuario"]) : 0,
                    IdReserva = row.Table.Columns.Contains("IdReserva") && row["IdReserva"] != DBNull.Value ? Convert.ToInt32(row["IdReserva"]) : idReserva,
                    Subtotal = row.Table.Columns.Contains("Subtotal") && row["Subtotal"] != DBNull.Value ? Convert.ToDecimal(row["Subtotal"]) : 0,
                    IVA = row.Table.Columns.Contains("IVA") && row["IVA"] != DBNull.Value ? Convert.ToDecimal(row["IVA"]) : 0,
                    Total = row.Table.Columns.Contains("Total") && row["Total"] != DBNull.Value ? Convert.ToDecimal(row["Total"]) : 0,
                    Fecha = row.Table.Columns.Contains("Fecha") && row["Fecha"] != DBNull.Value ? Convert.ToDateTime(row["Fecha"]).ToString("yyyy-MM-ddTHH:mm:ss.fffK") : DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffK"),
                    Estado = row.Table.Columns.Contains("Estado") ? row["Estado"]?.ToString() ?? "" : ""
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
    }
}
