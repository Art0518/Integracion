using Microsoft.AspNetCore.Mvc;

namespace Gateway.CafeSanJuan.Controllers
{
    [ApiController]
    [Route("api/gateway")]
  public class GatewayInfoController : ControllerBase
    {
        /// <summary>
  /// Obtener información del API Gateway
  /// </summary>
 [HttpGet("info")]
        [ProducesResponseType(200)]
        public IActionResult GetInfo()
        {
  return Ok(new
      {
          Nombre = "API Gateway - Café San Juan",
          Version = "1.0.0",
            Descripcion = "Gateway que centraliza todos los microservicios",
       Microservicios = new[]
      {
new { Nombre = "Reservas", URL = "https://reservas-production1.up.railway.app" },
    new { Nombre = "Facturas", URL = "https://factura-production-7d28.up.railway.app" },
    new { Nombre = "Usuarios", URL = "https://usuario-production1.up.railway.app" },
             new { Nombre = "Búsqueda", URL = "https://busqueda-production-70a5.up.railway.app" }
     }
     });
        }

   /// <summary>
   /// Verificar estado del Gateway (Health Check)
        /// </summary>
        [HttpGet("health")]
   [ProducesResponseType(200)]
 public IActionResult HealthCheck()
  {
 return Ok(new
   {
    Estado = "Healthy",
     Timestamp = DateTime.UtcNow,
   Uptime = Environment.TickCount64 / 1000 // segundos
      });
        }

    /// <summary>
        /// Listar todas las rutas configuradas en Ocelot (actualizadas)
        /// </summary>
        [HttpGet("routes")]
        [ProducesResponseType(200)]
        public IActionResult GetRoutes()
        {
    var routes = new[]
     {
            // Reservas
            new { Metodo = "POST", Ruta = "/api/reservas/hold", Microservicio = "Reservas", Descripcion = "Crear pre-reserva" },
                new { Metodo = "POST", Ruta = "/api/reservas/confirmar", Microservicio = "Reservas", Descripcion = "Confirmar reserva" },
new { Metodo = "GET", Ruta = "/api/reservas/{idReserva}", Microservicio = "Reservas", Descripcion = "Buscar reserva" },
    new { Metodo = "PUT", Ruta = "/api/reservas/cancelar/{idReserva}", Microservicio = "Reservas", Descripcion = "Cancelar reserva" },
    new { Metodo = "POST", Ruta = "/api/reservas/disponibilidad", Microservicio = "Reservas", Descripcion = "Verificar disponibilidad" },
 // Facturas (actualizados)
 new { Metodo = "POST", Ruta = "/api/facturas/emitir", Microservicio = "Facturas", Descripcion = "Emitir factura" },
 new { Metodo = "GET", Ruta = "/api/facturas/{idReserva}", Microservicio = "Facturas", Descripcion = "Obtener factura" },
 new { Metodo = "GET", Ruta = "/api/facturas/{idReserva}/pdf", Microservicio = "Facturas", Descripcion = "PDF de factura" },
 // Usuarios (actualizados)
 new { Metodo = "POST", Ruta = "/api/usuarios/registrar", Microservicio = "Usuarios", Descripcion = "Registrar usuario" },
 new { Metodo = "GET", Ruta = "/api/usuarios/listar", Microservicio = "Usuarios", Descripcion = "Listar usuarios" },
 // Búsqueda (actualizado)
 new { Metodo = "GET", Ruta = "/api/mesas/buscar", Microservicio = "Búsqueda", Descripcion = "Buscar mesas" }
            };

       return Ok(routes);
        }
    }
}
