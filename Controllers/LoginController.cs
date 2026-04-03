using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi2026.Settings;

namespace WebApi2026.Controllers
{
    public class LoginController : ControllerBase
    {

        private readonly TokenService _tokenService;

        public LoginController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] Dictionary<string, string> login)
        {
            if (login.TryGetValue("cpf", out var cpf) && login.TryGetValue("senha", out var senha))
            {
                if (cpf == "18894805760" && senha == "1234")
                {
                    var token = _tokenService.GerarToken(cpf);
                    return Ok(new { token });
                }

                return Unauthorized(new { Mensagem = "CPF ou senha incorretos" });
            }

            return BadRequest(new { Mensagem = "CPF e senha são obrigatórios" });
        }
    }
}
