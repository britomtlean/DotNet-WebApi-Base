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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file, [FromForm] Promocoes promocao)
        {
            try
            {
                var user = User.Identity?.Name;
                if (user == null) throw new Exception("Nenhum usuário vinculado a este login");

                var promocoes = await _service.CriarPromocao(file, promocao, user);
                return Ok(promocoes);
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
                var user = User.Identity?.Name;
                if (user == null) throw new Exception("Nenhum usuário vinculado a este login");

                var promocoes = await _service.ReturnPromocoes(user);
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
