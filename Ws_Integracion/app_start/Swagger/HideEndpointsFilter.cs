using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace Ws_Integracion.app_start.Swagger
{
    public class HideEndpointsFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var hide = apiDescription
                .ActionDescriptor
                .GetCustomAttributes<SwaggerHideAttribute>()
                .Any();

            if (hide)
            {
                // Elimina la operación del Swagger
                operation.tags.Clear();
                operation.summary = null;
                operation.description = null;
                operation.operationId = null;
                operation.parameters?.Clear();
                operation.responses?.Clear();
            }
        }
    }
}