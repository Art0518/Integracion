using System;
using System.Data;
using System.Web.Http;
using System.Web.Http.Description;
using Logica.Servicios;
using Ws_GIntegracionBus.DTOS;
using Ws_Integracion.dtos;

namespace Ws_GIntegracionBus.Controllers.V1
{
    [RoutePrefix("api/v1/integracion/restaurantes")]
    public class BusBusquedaController : ApiController
    {
        private readonly MesaLogica mesaLogica = new MesaLogica();

        /// <summary>
        /// Obtiene la lista de mesas. Los parámetros son únicamente para documentación del Swagger.
        /// </summary>
        /// <param name="capacidad">Capacidad mínima (solo documentado, no usado).</param>
        /// <param name="tipoMesa">Tipo de mesa (solo documentado, no usado).</param>
        /// <param name="estado">Estado de la mesa (solo documentado, no usado).</param>
        /// <returns>Listado completo de mesas.</returns>
        [HttpGet]
        [Route("search")]
        [ResponseType(typeof(BusquedaMesasSwaggerResponse))]
        public IHttpActionResult BuscarMesas(
            [FromUri] int? capacidad = null,
            [FromUri] string tipoMesa = null,
            [FromUri] string estado = null)
        {
            try
            {
                // 👇 TU LÓGICA ORIGINAL — NO TOCO NADA
                DataTable resultado = mesaLogica.ListarMesas();

                if (resultado == null || resultado.Rows.Count == 0)
                {
                    return Ok(new
                    {
                        mensaje = "No se encontraron mesas registradas.",
                        total = 0,
                        _links = new
                        {
                            self = new
                            {
                                href = Request.RequestUri.AbsoluteUri
                            },
                            createHold = new
                            {
                                href = $"{Request.RequestUri.GetLeftPart(UriPartial.Authority)}/api/v1/integracion/restaurantes/hold",
                                method = "POST"
                            }
                        }
                    });
                }

                // Mapear DataTable a List<MesaResponse> para incluir ImagenURL
                var mesasList = new System.Collections.Generic.List<MesaResponse>();
                foreach (DataRow r in resultado.Rows)
                {
                    var mesa = new MesaResponse
                    {
                        IdMesa = r.Table.Columns.Contains("IdMesa") && r["IdMesa"] != DBNull.Value ? Convert.ToInt32(r["IdMesa"]) : 0,
                        IdRestaurante = r.Table.Columns.Contains("IdRestaurante") && r["IdRestaurante"] != DBNull.Value ? Convert.ToInt32(r["IdRestaurante"]) : 0,
                        NumeroMesa = r.Table.Columns.Contains("NumeroMesa") && r["NumeroMesa"] != DBNull.Value ? Convert.ToInt32(r["NumeroMesa"]) : 0,
                        TipoMesa = r.Table.Columns.Contains("TipoMesa") ? r["TipoMesa"]?.ToString() ?? "" : "",
                        Capacidad = r.Table.Columns.Contains("Capacidad") && r["Capacidad"] != DBNull.Value ? Convert.ToInt32(r["Capacidad"]) : 0,
                        Estado = r.Table.Columns.Contains("Estado") ? r["Estado"]?.ToString() ?? "" : "",
                        Precio = r.Table.Columns.Contains("Precio") && r["Precio"] != DBNull.Value ? Convert.ToDecimal(r["Precio"]) : 0,
                        ImagenURL = r.Table.Columns.Contains("ImagenURL") ? r["ImagenURL"]?.ToString() ?? "" : ""
                    };

                    mesasList.Add(mesa);
                }

                return Ok(new
                {
                    mensaje = "Consulta de mesas realizada con éxito.",
                    total = mesasList.Count,
                    mesas = mesasList,
                    _links = new
                    {
                        self = new
                        {
                            href = Request.RequestUri.AbsoluteUri
                        },
                        createHold = new
                        {
                            href = $"{Request.RequestUri.GetLeftPart(UriPartial.Authority)}/api/v1/integracion/restaurantes/hold",
                            method = "POST"
                        },
                        reservar = new
                        {
                            href = $"{Request.RequestUri.GetLeftPart(UriPartial.Authority)}/api/v1/integracion/restaurantes/reservar",
                            method = "POST"
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Error al buscar mesas: " + ex.Message);
            }
        }
    }
}

