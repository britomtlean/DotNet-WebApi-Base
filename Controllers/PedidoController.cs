using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WebApi2026.Entities;
using WebApi2026.Interfaces;

namespace WebApi2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _service;

        public PedidoController(IPedidoService service)
        {
            _service = service;
        }

        // ROTAS

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Pedido pedido)
        {
            try
            {
                var resultado = await _service.AdicionarPedido(pedido);

                if (!resultado)
                {
                    return BadRequest("Erro ao adicionar pedido");
                }

                return Ok(new
                {
                    mensagem = "Pedido criado com sucesso"
                });
            }
            catch(Exception er)
            {
                {
                    return BadRequest($"{er.Message}");
                }
            }
        }

    }
}
