using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WebApi2026.Entities;
using WebApi2026.Interfaces;
using WebApi2026.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace WebApi2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _service;
        private readonly IHubContext<ChatHub> _hub;

        public PedidoController(IPedidoService service, IHubContext<ChatHub> hub)
        {
            _service = service;
            _hub = hub;
        }

        // ROTAS

        [HttpPost]
        public async Task<IActionResult> cofirmOrder([FromBody] Pedido pedido)
        {
            try
            {
                var resultado = await _service.AdicionarPedido(pedido);

                if (!resultado)
                {
                    return BadRequest("Erro ao confirmar pedido");
                }

                await _hub.Clients
                    .Group($"{pedido.ContatoCliente}")
                    .SendAsync(
                        "ReceiveMessage",
                        $"Pedido: {pedido.Id} confirmado!"
                    );

                return Ok("Pedido confirmado");
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
