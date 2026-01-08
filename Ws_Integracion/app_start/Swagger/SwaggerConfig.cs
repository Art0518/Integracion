using Swashbuckle.Application;
using System.Linq;
using System.Web.Http;
using Ws_GIntegracionBus.App_Start.Swagger;
using Ws_Integracion.app_start.Swagger;

namespace Ws_GIntegracionBus.App_Start
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var config = GlobalConfiguration.Configuration;

            config.EnableSwagger(c =>
            {
                // 🟦 VERSIÓN OFICIAL DE LA API
                c.SingleApiVersion("v1", "UnRinconEnSanJuan - Bus Integración REST");

                // 🟦 Mostrar rutas basadas en atributo (IMPORTANTE)
                c.UseFullTypeNameInSchemaIds();
                c.DocumentFilter<SwaggerBasePathFilter>();
                c.DocumentFilter<SwaggerHidePathsFilter>();
                c.SchemaFilter<HideModelSchemaFilter>();
                c.OperationFilter<HideEndpointsFilter>();
                //c.OperationFilter<Ws_GIntegracionBus.app_start.Swagger.HideFromSwaggerFilter>();
                c.DocumentFilter<OcultarTagsFilter>();
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                // 🟦 Describir enums como strings
                c.DescribeAllEnumsAsStrings();

                // 🟦 Incluir comentarios XML (documentación avanzada)
                var baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                var xmlFile = System.IO.Path.Combine(baseDirectory, "Ws_GIntegracionBus.XML");
                if (System.IO.File.Exists(xmlFile))
                {
                    c.IncludeXmlComments(xmlFile);
                }

                // 🟦 Soporte para HATEOAS (mostrar _links)
                c.SchemaFilter<SwaggerHateoasFilter>();



            })
            .EnableSwaggerUi(c =>
            {
                c.DocumentTitle("UnRinconEnSanJuan - API REST v1 (Bus Integración)");
                c.EnableDiscoveryUrlSelector();
                // c.InjectStylesheet(typeof(SwaggerConfig).Assembly, "Ws_GIntegracionBus.SwaggerTheme.css");
            });

        }
    }
}

