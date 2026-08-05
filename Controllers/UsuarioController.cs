using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization; //Autenticação
using WebApi2026.Entities;
using WebApi2026.Interfaces;

namespace WebApi2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            this._service = service;
        }

        //////////////////////////// ROUTERS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetUnique()
        {
            var cpf = User.Identity?.Name; //EXTRAI O CPF CONTIDO NO TOKEN
            var usuario = await _service.GetUnique(cpf);

            if (usuario == null)
                return NotFound(new { mensagem = "Usuário não encontrado" });


            return Ok(usuario);
        }
    }
}
