using Microsoft.AspNetCore.Mvc;
using System.Data;
using Logica.Servicios;
using Microservicio.Busqueda.DTOs;

namespace Microservicio.Busqueda.Controllers
{
    [ApiController]
    [Route("api/v1/integracion/restaurantes")]
    public class BusquedaController : ControllerBase
    {
     private readonly MesaLogica _mesaLogica = new MesaLogica();

        /// <summary>
   /// Obtiene la lista de mesas. Los parámetros son únicamente para documentación del Swagger.
   /// </summary>
        [HttpGet("search")]
      [ProducesResponseType(200)]
        public IActionResult BuscarMesas(
    [FromQuery] int? capacidad = null,
  [FromQuery] string tipoMesa = null,
  [FromQuery] string estado = null)
        {
  try
   {
        DataTable resultado = _mesaLogica.ListarMesas();

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
   href = $"{Request.Scheme}://{Request.Host}{Request.Path}"
     },
           createHold = new
{
  href = $"{Request.Scheme}://{Request.Host}/api/v1/integracion/restaurantes/hold",
    method = "POST"
}
          }
   });
            }

    // Mapear DataTable a List<MesaResponse> para incluir ImagenURL
       var mesasList = new List<MesaResponse>();
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
 href = $"{Request.Scheme}://{Request.Host}{Request.Path}"
          },
 createHold = new
{
  href = $"{Request.Scheme}://{Request.Host}/api/v1/integracion/restaurantes/hold",
method = "POST"
      },
   reservar = new
        {
  href = $"{Request.Scheme}://{Request.Host}/api/v1/integracion/restaurantes/reservar",
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
