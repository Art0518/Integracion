using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Gateway.CafeSanJuan.Swagger
{
 public class ProxyEndpointsDocumentFilter : IDocumentFilter
 {
 public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
 {
 // RESERVAS
 swaggerDoc.Paths.Add("/api/reservas/hold", new OpenApiPathItem
 {
 Operations = new Dictionary<OperationType, OpenApiOperation>
 {
 [OperationType.Post] = new OpenApiOperation
 {
 Summary = "Crear pre-reserva (proxy)",
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
 Responses = new OpenApiResponses { ["200"] = new OpenApiResponse { Description = "OK" } }
 }
 }
 });

 // BÚSQUEDA
 swaggerDoc.Paths.Add("/api/mesas/buscar", new OpenApiPathItem
 {
 Operations = new Dictionary<OperationType, OpenApiOperation>
 {
 [OperationType.Get] = new OpenApiOperation
 {
 Summary = "Buscar mesas (proxy)",
 Responses = new OpenApiResponses { ["200"] = new OpenApiResponse { Description = "OK" } }
 }
 }
 });
 }
 }
}
