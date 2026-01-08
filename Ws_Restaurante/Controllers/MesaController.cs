using System;
using System.Data;
using System.Web.Http;
using Logica.Servicios;
using GDatos.Entidades;

namespace Ws_GestionInterna.Controllers
{
    [RoutePrefix("api/mesas")]
    public class MesaController : ApiController
    {
        private readonly MesaLogica mesaLogica = new MesaLogica();

        // ✅ GET: /api/mesas
        [HttpGet]
        [Route("")]
        public IHttpActionResult ListarMesas()
        {
            try
            {
                DataTable mesas = mesaLogica.ListarMesas();
                return Ok(mesas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ GET: /api/mesas/disponibles?capacidad=4&tipoMesa=VIP
        [HttpGet]
        [Route("disponibles")]
        public IHttpActionResult ListarMesasDisponibles(int capacidad = 0, string tipoMesa = null)
        {
            try
            {
                DataTable mesas = mesaLogica.BuscarMesas(tipoMesa, capacidad, "DISPONIBLE");
                return Ok(mesas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ POST: /api/mesas/gestionar
        [HttpPost]
        [Route("gestionar")]
        public IHttpActionResult GestionarMesa([FromBody] Mesa m)
        {
            try
            {
                // 👇 aquí ya no llamamos al DAO directamente
                // sino a la capa Lógica, como hicimos con los platos
                mesaLogica.GestionarMesa(m);

                string mensaje = m.IdMesa > 0 ? "Mesa actualizada correctamente" : "Mesa registrada correctamente";
                return Ok(new { mensaje });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}/estado")]
        public IHttpActionResult ActualizarEstado(int id, [FromBody] GDatos.Entidades.Mesa dto)
        {
            try
            {
                mesaLogica.ActualizarEstado(id, dto.Estado);
                return Ok(new { mensaje = "Estado actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // ✅ GET: /api/mesas/{id}
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult ObtenerMesa(int id)
        {
            try
            {
                DataTable todas = mesaLogica.ListarMesas();
                DataRow[] filtro = todas.Select($"IdMesa = {id}");

                if (filtro.Length == 0)
                    return NotFound();

                var mesa = filtro[0];
                return Ok(new
                {
                    IdMesa = mesa["IdMesa"],
                    //IdRestaurante = mesa["IdRestaurante"],
                    NumeroMesa = mesa["NumeroMesa"],
                    TipoMesa = mesa["TipoMesa"],
                    Capacidad = mesa["Capacidad"],
                    Precio = mesa["Precio"],
                    ImagenURL = mesa["ImagenURL"],
                    Estado = mesa["Estado"]
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ DELETE: /api/mesas/{id}
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult EliminarMesa(int id)
        {
            try
            {
                mesaLogica.ActualizarEstado(id, "INACTIVA");
                return Ok(new { mensaje = "Mesa inactivada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}