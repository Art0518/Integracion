using Logica.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Ws_GIntegracionBus.DTOS;

namespace Ws_GIntegracionBus.Controllers.V1
{
    [RoutePrefix("api/v1/integracion/restaurantes")]
    public class BusFacturaController : ApiController
    {
        private readonly FacturaLogica facturaLogica = new FacturaLogica();

        /// <summary>
        /// Genera una factura para una reserva específica.
        /// </summary>
        /// <param name="body">Objeto JSON con IdUsuario, IdReserva, Subtotal, IVA y Total.</param>
        /// <returns>Detalle de la factura emitida.</returns>
        [HttpPost]
        [Route("invoices")]
        [ResponseType(typeof(FacturaResponse))]
        public IHttpActionResult EmitirFactura([FromBody] FacturaRequest body)
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

                // Llamada a la lógica para generar la factura (retorna DataTable con la respuesta del SP)
                DataTable dt = facturaLogica.GenerarFactura(idReserva, correo, nombre, tipoIdentificacion, identificacion, valor);

                if (dt == null || dt.Rows.Count == 0)
                    return BadRequest("No se obtuvo respuesta al generar la factura.");

                var row = dt.Rows[0];
                var response = new FacturaResponse();
                response.Mensaje = row.Table.Columns.Contains("Mensaje") ? row["Mensaje"]?.ToString() ?? "" : "";
                response.IdUsuario = row.Table.Columns.Contains("IdUsuario") && row["IdUsuario"] != DBNull.Value ? Convert.ToInt32(row["IdUsuario"]) : 0;
                response.IdReserva = row.Table.Columns.Contains("IdReserva") && row["IdReserva"] != DBNull.Value ? Convert.ToInt32(row["IdReserva"]) : idReserva;
                response.Subtotal = row.Table.Columns.Contains("Subtotal") && row["Subtotal"] != DBNull.Value ? Convert.ToDecimal(row["Subtotal"]) : 0;
                response.IVA = row.Table.Columns.Contains("IVA") && row["IVA"] != DBNull.Value ? Convert.ToDecimal(row["IVA"]) : 0;
                response.Total = row.Table.Columns.Contains("Total") && row["Total"] != DBNull.Value ? Convert.ToDecimal(row["Total"]) : 0;
                response.Fecha = row.Table.Columns.Contains("Fecha") && row["Fecha"] != DBNull.Value ? Convert.ToDateTime(row["Fecha"]).ToString("yyyy-MM-ddTHH:mm:ss.fffK") : DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffK");
                response.Estado = row.Table.Columns.Contains("Estado") ? row["Estado"]?.ToString() ?? "" : "";

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
        /// <param name="idFactura">ID de la factura.</param>
        /// <returns>Detalles de la factura.</returns>
        [HttpGet]
        [Route("invoices/{idFactura}")]
        [ResponseType(typeof(FacturaResponse))]
        public IHttpActionResult ObtenerFactura(int idFactura)
        {
            try
            {
                var factura = facturaLogica.ObtenerFacturaPorId(idFactura);
                if (factura == null)
                {
                    return NotFound();
                }

                var detalles = facturaLogica.ObtenerDetallesDeFactura(idFactura);

                // Transformar las filas de detalles en un formato más accesible
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
        /// <param name="idFactura">ID de la factura.</param>
        /// <returns>Archivo PDF de la factura.</returns>
        [HttpGet]
        [Route("invoices/{idFactura}/pdf")]
        public IHttpActionResult ObtenerPdfFactura(int idFactura)
        {
            try
            {
                // Construir la URI de la factura en PDF
                string baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                var pdfUri = $"{baseUrl}/api/v1/integracion/restaurantes/invoices/{idFactura}/document.pdf";

                // Responder con el mensaje adecuado y el enlace al PDF
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
