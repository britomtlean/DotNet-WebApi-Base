using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApi2026.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class Teste : ControllerBase
    {

        private static readonly string[] clubesRJ = new[] { "Botafogo", "Flamengo", "Vasco", "Fluminense" };


        [HttpGet]
        public IActionResult GetHTML()
        {
            var caminho = Path.Combine(Directory.GetCurrentDirectory(), "Views", "index.html");
            return PhysicalFile(caminho, "text/html");
        }


    }
}
