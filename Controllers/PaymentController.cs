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

                LineItems = new List<SessionLineItemOptions>
                {
                    //ITEMS
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "brl",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Carrinho"
                            },
                            UnitAmount = 5000
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
