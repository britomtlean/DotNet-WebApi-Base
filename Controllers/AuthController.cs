using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi2026.Interfaces; //INTERFACE
using WebApi2026.Types; //TYPE
using WebApi2026.Entities; //Entitie

namespace WebApi2026.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Usuario newUser)
        {
            try
            {

            var message = await _service.Register(newUser);
            return Ok(message);

            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                return BadRequest(new { mensagem = er.Message });
            }
        }

        /*
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
        */

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            try
            {
                var token = await _service.Login(login);
                return Ok(token);
            }
            catch(Exception er)
            {
                return Unauthorized(er.Message);
            }

        }

    }
}
