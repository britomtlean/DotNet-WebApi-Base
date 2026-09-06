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
        public async Task<IActionResult> Register([FromBody] Login newLogin)
        {
            try
            {

            var message = await _service.Register(newLogin);
            return Ok(message);

            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                return BadRequest(new { mensagem = er.Message });
            }
        }


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
