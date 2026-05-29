using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout; //STRIP

namespace WebApi2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateCheckout()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                //CARRINHO
                LineItems = new List<SessionLineItemOptions>
                {
                    //PRODUTO
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "brl",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Item 1"
                            },
                            UnitAmount = 100
                        },
                        Quantity = 1
                    },
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "brl",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "item 2"
                            },
                            UnitAmount = 200
                        },
                        Quantity = 1
                    },
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "brl",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "item 3"
                            },
                            UnitAmount = 300
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = "http://localhost:5173/sucesso",
                CancelUrl = "http://localhost:5173/cancelado"
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
