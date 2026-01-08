using Swashbuckle.Swagger;
using System.Linq;
using System.Web.Http.Description;
using Ws_Integracion.app_start.Swagger;

namespace Ws_GIntegracionBus.app_start.Swagger
{
    public class HideFromSwaggerFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            bool hide = apiDescription
                .ActionDescriptor
                .GetCustomAttributes<SwaggerHideAttribute>()
                .Any();

            if (hide)
            {
                // Oculta el endpoint completamente
                operation.tags = null;
                operation.summary = null;
                operation.description = null;
                operation.parameters = null;
            }
        }
    }
}
