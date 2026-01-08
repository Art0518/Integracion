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
             new { Nombre = "Disponibilidad", URL = "https://disponibilidad-production-1469.up.railway.app" },
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
        /// Listar todas las rutas configuradas en Ocelot
        /// </summary>
        [HttpGet("routes")]
        [ProducesResponseType(200)]
        public IActionResult GetRoutes()
        {
    var routes = new[]
     {
            new { Metodo = "POST", Ruta = "/api/v1/integracion/restaurantes/hold", Microservicio = "Reservas", Descripcion = "Crear pre-reserva" },
                new { Metodo = "POST", Ruta = "/api/v1/integracion/restaurantes/book", Microservicio = "Reservas", Descripcion = "Confirmar reserva" },
new { Metodo = "GET", Ruta = "/api/v1/integracion/restaurantes/reservas/{id}", Microservicio = "Reservas", Descripcion = "Buscar reserva" },
    new { Metodo = "POST", Ruta = "/api/v1/integracion/restaurantes/cancelar", Microservicio = "Reservas", Descripcion = "Cancelar reserva" },
 new { Metodo = "POST", Ruta = "/api/v1/integracion/restaurantes/invoices", Microservicio = "Facturas", Descripcion = "Crear factura" },
 new { Metodo = "GET", Ruta = "/api/v1/integracion/restaurantes/invoices/{id}", Microservicio = "Facturas", Descripcion = "Obtener factura" },
         new { Metodo = "GET", Ruta = "/api/v1/integracion/restaurantes/invoices/{id}/pdf", Microservicio = "Facturas", Descripcion = "PDF de factura" },
      new { Metodo = "POST", Ruta = "/api/v1/integracion/restaurantes/usuarios", Microservicio = "Usuarios", Descripcion = "Crear usuario" },
         new { Metodo = "POST", Ruta = "/api/v1/integracion/restaurantes/availability", Microservicio = "Disponibilidad", Descripcion = "Verificar disponibilidad" },
       new { Metodo = "GET", Ruta = "/api/v1/integracion/restaurantes/search", Microservicio = "Búsqueda", Descripcion = "Buscar mesas" }
            };

       return Ok(routes);
        }
    }
}
