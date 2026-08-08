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
            try
            {
                var user = User.Identity?.Name; //EXTRAI O CPF CONTIDO NO TOKEN
                if (user == null) throw new Exception("Nenhum usuário vinculado a este login");

                var usuario = await _service.GetUnique(user);

                if (usuario == null) throw new Exception("Usuário não encontrado");

                return Ok(usuario);
            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
                return BadRequest(er.Message);
            }

        }
    }
}
