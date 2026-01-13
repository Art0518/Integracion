using Microsoft.AspNetCore.Mvc;
using System.Data;
using Logica.Servicios;
using Microservicio.Busqueda.DTOs;

namespace Microservicio.Busqueda.Controllers
{
    [ApiController]
    [Route("api/mesas")]
    public class BusquedaController : ControllerBase
    {
        private readonly MesaLogica _mesaLogica = new MesaLogica();

        /// <summary>
        /// Busca mesas aplicando filtros opcionales de capacidad, tipo de mesa y estado.
        /// </summary>
        /// <param name="capacidad">Capacidad mínima de la mesa (opcional)</param>
        /// <param name="tipoMesa">Tipo de mesa a filtrar (opcional)</param>
        /// <param name="estado">Estado de la mesa a filtrar (opcional)</param>
        /// <returns>Lista de mesas que cumplen con los criterios de búsqueda</returns>
        [HttpGet("buscar")]
        [ProducesResponseType(typeof(List<MesaResponse>), 200)]
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
                    return Ok(new List<MesaResponse>());
                }

                // Mapear DataTable a List<MesaResponse>
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

                // Aplicar filtros
                var mesasFiltradas = mesasList.AsEnumerable();

                // Filtro por capacidad (mayor o igual)
                if (capacidad.HasValue && capacidad.Value > 0)
                {
                    mesasFiltradas = mesasFiltradas.Where(m => m.Capacidad >= capacidad.Value);
                }

                // Filtro por tipo de mesa (comparación sin distinguir mayúsculas/minúsculas)
                if (!string.IsNullOrWhiteSpace(tipoMesa))
                {
                    mesasFiltradas = mesasFiltradas.Where(m =>
                      m.TipoMesa.Equals(tipoMesa, StringComparison.OrdinalIgnoreCase));
                }

                // Filtro por estado (comparación sin distinguir mayúsculas/minúsculas)
                if (!string.IsNullOrWhiteSpace(estado))
                {
                    mesasFiltradas = mesasFiltradas.Where(m =>
                       m.Estado.Equals(estado, StringComparison.OrdinalIgnoreCase));
                }

                return Ok(mesasFiltradas.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest("Error al buscar mesas: " + ex.Message);
            }
        }
    }
}
