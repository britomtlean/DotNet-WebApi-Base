using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout; //STRIP
using WebApi2026.Entities;
using System.Text.Json;
using WebApi2026.Interfaces;

namespace WebApi2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPedidoService _service;

        public PaymentController(IPedidoService service)
        {
            this._service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckout([FromBody] Pedido pedido)
        {
            Console.WriteLine($"_______________________________________________________");
            // CONVERTE EM JSON
            Console.WriteLine($"SOLICITAÇÃO DE PAGAMENTO ONLINE\nPEDIDO: {pedido.Id} RECEBIDO");
            Console.WriteLine("________________________________________________________");

            try
            {
                /////////////// SALVAR PEDIDO \\\\\\\\\\\\\\\\\
                await this._service.AdicionarPedido(pedido);
                Console.WriteLine($"Pedido registrado na database");
                //////////////////////////////////////////////

                ///////////// LISTA DE PRODUTOS \\\\\\\\\\\\\\\\\\\
                var lineItems = new List<SessionLineItemOptions>();

                foreach (var produto in pedido.Produtos)
                {
                    lineItems.Add(
                    new SessionLineItemOptions
                    {
                        Quantity = produto.Quantidade,

                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "brl",

                            // Stripe trabalha em centavos
                            UnitAmount = (long)(produto.ValorUnitario * 100),

                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = produto.Nome
                            }
                        }
                    });
                }

                /////////////////////////////////////////////////

                //////////////// CONFIGURAÇÕES \\\\\\\\\\\\\\\\\\
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                    {
                        "card"
                    },
                    LineItems = lineItems, //CARRINHO
                    Mode = "payment",
                    SuccessUrl = "https://www.google.com/?zx=1782782819848",
                    CancelUrl = "https://www.bing.com/?cc=br",
                    Metadata = new Dictionary<string, string>
                    {
                        { "pedido", pedido.Id }
                    },
                    PaymentIntentData = new SessionPaymentIntentDataOptions
                    {
                        Metadata = new Dictionary<string, string>
                        {
                            { "pedido", pedido.Id }
                        }
                    }
                };
                //////////////////////////////////////////////////

                var service = new SessionService();
                Session session = service.Create(options);

                return Ok(new
                {
                    url = session.Url
                });

            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                return BadRequest(error.Message);
            }

        }
    }
}
