using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;

namespace Ws_GIntegracionBus.App_Start.Swagger
{
    public class SwaggerHidePathsFilter : IDocumentFilter
    {
        // Rutas exactas que deseas OCULTAR
        private readonly string[] pathsToHide =
        {
        "/api/reserva",                 // ejemplo si estaba listado
        "/api/reserva/{id}",            // get por id interno
        "/api/usuario/login",           // login viejo
        "/api/usuario/listar",          // listar usuarios
        "/api/usuario/{id}",            // get por id interno
        "/api/detallefactura",          // controlador que NO quieres
        "/api/mesa",
        "/api/pago",
        "/api/plato",
        "/api/promocion"
        // Puedes agregar más
    };

        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            foreach (var path in swaggerDoc.paths.Keys.ToList())
            {
                if (pathsToHide.Any(h => path.ToLower().StartsWith(h.ToLower())))
                {
                    swaggerDoc.paths.Remove(path);
                }
            }
        }
    }
}
