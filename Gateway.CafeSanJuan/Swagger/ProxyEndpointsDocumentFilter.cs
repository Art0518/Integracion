using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Gateway.CafeSanJuan.Swagger
{
 public class ProxyEndpointsDocumentFilter : IDocumentFilter
 {
 public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
 {
 // Limpiar tags existentes y definir los nuestros
 swaggerDoc.Tags = new List<OpenApiTag>
 {
 new OpenApiTag { Name = "Usuarios", Description = "Operaciones de usuarios" },
 new OpenApiTag { Name = "Reservas", Description = "Operaciones de reservas" },
 new OpenApiTag { Name = "Facturas", Description = "Operaciones de facturación" },
 new OpenApiTag { Name = "Mesas", Description = "Operaciones de búsqueda de mesas" }
 };

 // RESERVAS
 swaggerDoc.Paths.Add("/api/reservas/hold", new OpenApiPathItem
 {
 Operations = new Dictionary<OperationType, OpenApiOperation>
 {
 [OperationType.Post] = new OpenApiOperation
 {
 Summary = "Crear pre-reserva (proxy)",
 Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Reservas" } },
 Responses = new OpenApiResponses { ["200"] = new OpenApiResponse { Description = "OK" } }
 }
 }
 });
 swaggerDoc.Paths.Add("/api/reservas/confirmar", new OpenApiPathItem
 {
 Operations = new Dictionary<OperationType, OpenApiOperation>
 {
 [OperationType.Post] = new OpenApiOperation
 {
 Summary = "Confirmar reserva (proxy)",
 Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Reservas" } },
 Responses = new OpenApiResponses { ["200"] = new OpenApiResponse { Description = "OK" } }
 }
 }
 });
 swaggerDoc.Paths.Add("/api/reservas/{idReserva}", new OpenApiPathItem
 {
 Operations = new Dictionary<OperationType, OpenApiOperation>
 {
 [OperationType.Get] = new OpenApiOperation
 {
 Summary = "Buscar reserva (proxy)",
 Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Reservas" } },
 Parameters = new List<OpenApiParameter>
 {
 new OpenApiParameter
 {
 Name = "idReserva",
 In = ParameterLocation.Path,
 Required = true,
 Schema = new OpenApiSchema { Type = "string" }
 }
 },
 Responses = new OpenApiResponses { ["200"] = new OpenApiResponse { Description = "OK" } }
 }
 }
 });
 swaggerDoc.Paths.Add("/api/reservas/cancelar/{idReserva}", new OpenApiPathItem
 {
 Operations = new Dictionary<OperationType, OpenApiOperation>
 {
 [OperationType.Put] = new OpenApiOperation
 {
 Summary = "Cancelar reserva (proxy)",
 Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Reservas" } },
 Parameters = new List<OpenApiParameter>
 {
 new OpenApiParameter
 {
 Name = "idReserva",
 In = ParameterLocation.Path,
 Required = true,
 Schema = new OpenApiSchema { Type = "string" }
 }
 },
 Responses = new OpenApiResponses { ["200"] = new OpenApiResponse { Description = "OK" } }
 }
 }
 });
 swaggerDoc.Paths.Add("/api/reservas/disponibilidad", new OpenApiPathItem
 {
 Operations = new Dictionary<OperationType, OpenApiOperation>
 {
 [OperationType.Post] = new OpenApiOperation
 {
 Summary = "Verificar disponibilidad (proxy)",
 Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Reservas" } },
 Responses = new OpenApiResponses { ["200"] = new OpenApiResponse { Description = "OK" } }
 }
 }
 });

 // FACTURAS
 swaggerDoc.Paths.Add("/api/facturas/emitir", new OpenApiPathItem
 {
 Operations = new Dictionary<OperationType, OpenApiOperation>
 {
 [OperationType.Post] = new OpenApiOperation
 {
 Summary = "Emitir factura (proxy)",
 Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Facturas" } },
 Responses = new OpenApiResponses { ["200"] = new OpenApiResponse { Description = "OK" } }
 }
 }
 });
 swaggerDoc.Paths.Add("/api/facturas/{idReserva}", new OpenApiPathItem
 {
 Operations = new Dictionary<OperationType, OpenApiOperation>
 {
 [OperationType.Get] = new OpenApiOperation
 {
 Summary = "Obtener factura (proxy)",
 Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Facturas" } },
 Parameters = new List<OpenApiParameter>
 {
 new OpenApiParameter
 {
 Name = "idReserva",
 In = ParameterLocation.Path,
 Required = true,
 Schema = new OpenApiSchema { Type = "string" }
 }
 },
 Responses = new OpenApiResponses { ["200"] = new OpenApiResponse { Description = "OK" } }
 }
 }
 });
 swaggerDoc.Paths.Add("/api/facturas/{idReserva}/pdf", new OpenApiPathItem
 {
 Operations = new Dictionary<OperationType, OpenApiOperation>
 {
 [OperationType.Get] = new OpenApiOperation
 {
 Summary = "PDF de factura (proxy)",
 Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Facturas" } },
 Parameters = new List<OpenApiParameter>
 {
 new OpenApiParameter
 {
 Name = "idReserva",
 In = ParameterLocation.Path,
 Required = true,
 Schema = new OpenApiSchema { Type = "string" }
 }
 },
 Responses = new OpenApiResponses { ["200"] = new OpenApiResponse { Description = "OK" } }
 }
 }
 });

 // USUARIOS
 swaggerDoc.Paths.Add("/api/usuarios/registrar", new OpenApiPathItem
 {
 Operations = new Dictionary<OperationType, OpenApiOperation>
 {
 [OperationType.Post] = new OpenApiOperation
 {
 Summary = "Registrar usuario (proxy)",
 Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Usuarios" } },
 Responses = new OpenApiResponses { ["200"] = new OpenApiResponse { Description = "OK" } }
 }
 }
 });
 swaggerDoc.Paths.Add("/api/usuarios/listar", new OpenApiPathItem
 {
 Operations = new Dictionary<OperationType, OpenApiOperation>
 {
 [OperationType.Get] = new OpenApiOperation
 {
 Summary = "Listar usuarios (proxy)",
 Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Usuarios" } },
 Responses = new OpenApiResponses { ["200"] = new OpenApiResponse { Description = "OK" } }
 }
 }
 });

 // MESAS (Búsqueda)
 swaggerDoc.Paths.Add("/api/mesas/buscar", new OpenApiPathItem
 {
 Operations = new Dictionary<OperationType, OpenApiOperation>
 {
 [OperationType.Get] = new OpenApiOperation
 {
 Summary = "Buscar mesas (proxy)",
 Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Mesas" } },
 Responses = new OpenApiResponses { ["200"] = new OpenApiResponse { Description = "OK" } }
 }
 }
 });
 }
 }
}
