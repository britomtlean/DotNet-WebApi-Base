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
        public IActionResult CreateCheckout([FromBody] Pedido pedido)
        {

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


            var pedidoJson = JsonSerializer.Serialize(pedido);
            Console.WriteLine(pedidoJson);


            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                //CARRINHO
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "http://localhost:5174/sucesso",
                CancelUrl = "http://localhost:5174/cancelado",
                Metadata = new Dictionary<string, string>
                {
                    { "pedido", pedido.Id }
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
