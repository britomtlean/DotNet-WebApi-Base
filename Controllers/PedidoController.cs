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

        // LOJA CONFIRMA O PEDIDO PAGO NA ENTREGA
        /* LOJA SALVA O PEDIDO NO BANCO
        [HttpPost]
        public async Task<IActionResult> SalvarPedido([FromBody] Pedido pedido)
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
                        $"Pedido confirmado!"
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
        */





        [HttpGet]
        public async Task<IActionResult> RetornarPedidos()
        {
            try
            {
                var pedidos = await _service.RetornarPedido();
                return Ok(pedidos);
            }
            catch(Exception er)
            {
                return BadRequest(er.Message);
            }
        }

        [HttpPut("confirmar")]
        public async Task<IActionResult> ConfirmarPedido(Pedido pedido)
        {
            try
            {
                var service = await this._service.ConfirmarPedido(pedido);

                await _hub.Clients
                    .Group($"{pedido.ContatoCliente}")
                    .SendAsync(
                        "ReceiveMessage",
                        $"Pedido confirmado!"
                    );

                return Ok("Pedido confirmado com sucesso!");
            }
            catch(Exception er)
            {
                return BadRequest(er.Message);
            }
        }

        [HttpPut("cancelar")]
        public async Task<IActionResult> CancelarPedido(Pedido pedido)
        {
            try
            {
                var service = await this._service.CancelarPedido(pedido);

                await _hub.Clients
                    .Group($"{pedido.ContatoCliente}")
                    .SendAsync(
                        "ReceiveMessage",
                        $"Pedido cancelado!"
                    );

                return Ok("Pedido cancelado com sucesso!"); ;
            }
            catch (Exception er)
            {
                return BadRequest(er.Message);
            }
        }

    }
}
