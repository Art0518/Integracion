using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ws_Integracion.app_start.Swagger
{
    public class HideModelSchemaFilter : ISchemaFilter
    {
        private readonly string[] modelosOcultos = new[]
        {
        "Usuario",
        "UsuarioRequest",
        "UsuarioResponse"
    };

        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            if (modelosOcultos.Contains(type.Name))
            {
                schemaRegistry.Definitions.Remove(type.Name);
            }
        }
    }
}