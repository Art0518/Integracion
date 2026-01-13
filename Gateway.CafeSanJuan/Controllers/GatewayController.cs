using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Gateway.CafeSanJuan.Controllers
{
    [ApiController]
    [Route("api/gateway")]
    public class GatewayController : ControllerBase
    {
        [HttpGet("ocelot")]
        public IActionResult GetOcelot()
        {
            var path = Path.Combine(System.AppContext.BaseDirectory, "ocelot.json");
            if (!System.IO.File.Exists(path))
            {
                // Try content root
                path = Path.Combine(Directory.GetCurrentDirectory(), "ocelot.json");
            }

            if (!System.IO.File.Exists(path))
            {
                return NotFound("ocelot.json not found");
            }

            var content = System.IO.File.ReadAllText(path);
            return Content(content, "application/json");
        }
    }
}
