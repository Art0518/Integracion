using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ws_Integracion.app_start.Swagger
{

    [AttributeUsage(AttributeTargets.Method)]
    public class SwaggerHideAttribute : Attribute
    {
    }
}