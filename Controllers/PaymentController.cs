using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout; //STRIP
using WebApi2026.Entities;
using System.Text.Json;

namespace WebApi2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {

        [HttpPost]
        public IActionResult CreateCheckout()
        {

            var pedido = new Pedido
            {
                Produtos = new List<ProdutoPedido>
                {
                    new ProdutoPedido
                    {
                        ProdutoId = "6a10d5b7ae6f124854579c0e",
                        Nome = "Top Jet",
                        Quantidade = 1,
                        ValorUnitario = 10,
                        Subtotal = 10
                    }
                },

                ValorTotal = 10,

                NomeCliente = "Teste Cliente 1",

                ContatoCliente = "123",

                EnderecoCliente = "Rua X"
            };

            var pedidoJson = JsonSerializer.Serialize(pedido);


            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                //CARRINHO
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Quantity = 1,

                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "brl",

                            UnitAmount = 1000,

                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Top Jet"
                            }
                        }
                    }
                },
                Mode = "payment",
                SuccessUrl = "https://dotnet-webapi-base-production.up.railway.app/sucesso",
                CancelUrl = "https://dotnet-webapi-base-production.up.railway.app/cancelado",
                Metadata = new Dictionary<string, string>
                {
                    { "pedido", pedidoJson }
                }
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Ok(new
            {
                url = session.Url
            });
        }
    }
}
