using Swashbuckle.Swagger;
using System.Linq;
using System.Web.Http.Description;

namespace Ws_Integracion.app_start.Swagger
{
    public class OcultarTagsFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            // 📌 Controllers que quieres ocultar (del proyecto Ws_Restaurante)
            var namespacesAEliminar = new[]
            {
                "Ws_GestionInterna.Controllers",
                "Ws_Restaurante.Controllers"
            };

            // 🔥 Quitar paths que pertenecen a esos controllers
            var pathsAEliminar = swaggerDoc.paths
                .Where(p =>
                {
                    var descripcion = apiExplorer.ApiDescriptions
                        .FirstOrDefault(api => "/" + api.RelativePath == p.Key);

                    if (descripcion == null) return false;

                    var ns = descripcion.ActionDescriptor.ControllerDescriptor.ControllerType.Namespace;

                    return namespacesAEliminar.Any(n => ns != null && ns.StartsWith(n));
                })
                .Select(p => p.Key)
                .ToList();

            foreach (var path in pathsAEliminar)
                swaggerDoc.paths.Remove(path);

            // 🔥 Ocultar tags (solo si el controller pertenece a los namespaces anteriores)
            swaggerDoc.tags = swaggerDoc.tags?
                .Where(tag =>
                {
                    // obtener controllers que deben mostrarse
                    var controladoresVisibles = apiExplorer.ApiDescriptions
                        .Where(api =>
                        {
                            var ns = api.ActionDescriptor.ControllerDescriptor.ControllerType.Namespace;
                            return !namespacesAEliminar.Any(n => ns != null && ns.StartsWith(n));
                        })
                        .Select(api => api.ActionDescriptor.ControllerDescriptor.ControllerName)
                        .Distinct()
                        .ToList();

                    return controladoresVisibles.Contains(tag.name);
                })
                .ToList();
        }
    }
}
