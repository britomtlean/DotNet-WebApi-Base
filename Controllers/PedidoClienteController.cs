using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi2026.Interfaces;
using WebApi2026.Entities;

namespace WebApi2026.Controllers
{
    [Route("api/[controller]")]
    public class PedidoClienteController : Controller
    {
        private readonly IPedidoClienteService _service;

        public PedidoClienteController(IPedidoClienteService service)
        {
            _service = service;
        }


        //***************** ROTAS *************************//

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var user = User.Identity?.Name;
                if (user == null) throw new Exception("Nenhum usuário vinculado a este login");

                var response = await _service.SelectAll(user);
                return Ok(response);
            }
            catch(Exception er)
            {
                Console.WriteLine(er.ToString());
                return BadRequest(er.Message);
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PedidoCliente data)
        {
            try
            {
                var user = User.Identity?.Name;
                if (user == null) throw new Exception("Nenhum usuário vinculado a este login");

                var response = await _service.Insert(user, data);
                return Ok(response);
            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
                return BadRequest(er.Message);
            }
        }

    }
}
