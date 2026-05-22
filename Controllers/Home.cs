using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApi2026.Controllers
{

    [ApiController]
    [Route("")]
    public class Teste : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHTML()
        {
            var caminho = Path.Combine(Directory.GetCurrentDirectory(), "Public", "Views", "index.html");

            // Enviar html
            return PhysicalFile(caminho, "text/html");
        }


    }
}
