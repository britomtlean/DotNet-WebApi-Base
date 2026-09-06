using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApi2026.Entities;
using WebApi2026.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApi2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromocoesController : ControllerBase
    {
        private readonly IPromocoesService _service;

        public PromocoesController(IPromocoesService service)
        {
            this._service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file, [FromForm] Promocoes promocao)
        {
            try
            {
                var response = await _service.CriarPromocao(file, promocao);
                return Ok(response);
            }
            catch(Exception er)
            {
                Console.WriteLine(er.ToString());
                return BadRequest(er.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SendAll()
        {
            try
            {
                var login = User.Identity?.Name;
                if (login == null) throw new Exception("Nenhum usuário vinculado a este login");

                var promocoes = await _service.ReturnPromocoes(login);
                return Ok(promocoes);
            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
                return BadRequest(er.Message);
            }
        }


    }
}
