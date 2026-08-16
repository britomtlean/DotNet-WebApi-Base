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
        private readonly IHubContext<SignalRSettings> _hub;
        private readonly HttpClient _httpClient;

        private readonly IProdutosService _serviceProduto;

        public PedidoController(IPedidoService service, IHubContext<SignalRSettings> hub, IHttpClientFactory httpClientFactory, IProdutosService serviceProduto)
        {
            _service = service;
            _hub = hub;
            _httpClient = httpClientFactory.CreateClient("apiPDF");
            _serviceProduto = serviceProduto;
        }


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
                Console.WriteLine(er.ToString());
                return BadRequest(er.Message);
            }
        }

        [HttpPut("confirmar/{id}")]
        public async Task<IActionResult> ConfirmarPedido([FromRoute] string id)
        {
            try
            {
                var pedido = await this._service.ConfirmarPedido(id);

                try
                {
                    var res = await _httpClient.PostAsJsonAsync("gerarPDF", pedido);

                    if (!res.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Erro ao gerar pdf");
                        Console.WriteLine(await res.Content.ReadAsStringAsync());
                        throw new Exception("Erro ao gerar PDF. Pedido cancelado");
                    }
                }
                catch(Exception er)
                {
                    Console.WriteLine(er.ToString());

                    await this._service.CancelarPedido(id);
                    await _serviceProduto.EntradaEstoque(pedido.Produtos);

                    await _hub.Clients
                        .Group($"{pedido.ContatoCliente}")
                        .SendAsync(
                            "ReceiveMessage",
                            $"Pedido cancelado!"
                        );
                    throw new Exception(er.Message);
                }

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
                Console.WriteLine(er.ToString());
                return BadRequest(er.Message);
            }
        }

        [HttpPut("cancelar/{id}")]
        public async Task<IActionResult> CancelarPedido([FromRoute] string id)
        {
            try
            {
                var pedido = await this._service.CancelarPedido(id);

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
                Console.WriteLine(er.ToString());
                return BadRequest(er.Message);
            }
        }

    }
}
