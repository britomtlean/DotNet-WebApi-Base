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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Usuario usuario)
        {
            if (usuario == null)
                return BadRequest("Dados inválidos");

            await _service.Create(usuario);

            return Ok(usuario);
        }

        [Authorize]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _service.GetAll();

            return Ok(usuarios);

        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetUnique()
        {
            var cpf = User.Identity?.Name;
            var usuario = await _service.GetUnique(cpf);

            if (usuario == null)
                return NotFound(new { mensagem = "Usuário não encontrado" });


            return Ok(usuario);
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _service.Delete(id));
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromBody] string nome, string id)
        {
            await _service.UpdateNome(id, nome);
            return Ok("Atualizado com sucesso");
        }
    }
}
