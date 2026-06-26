using Microsoft.AspNetCore.Mvc;
using Stripe;
using Microsoft.AspNetCore.SignalR;
using WebApi2026.Interfaces;
using WebApi2026.Hubs;
using WebApi2026.Entities;
using System.Text.Json;

[ApiController]
[Route("api/stripe/webhook")]
public class StripeWebhookController : ControllerBase
{
    private readonly string endpointSecret = "whsec_3AK6E4iZs9TOb0WpVCixISS1Zcf6Z4jQ";
    private readonly IPedidoService _service;
    private readonly IHubContext<ChatHub> _hub;

    public StripeWebhookController(IPedidoService service, IHubContext<ChatHub> hub)
    {
        _service = service;
        _hub = hub;
    }

    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                endpointSecret
            );

            // Pagamento concluído
            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                //BODY
                //var pedidoJson = session.Metadata["pedido"];
                //var pedido = JsonSerializer.Deserialize<Pedido>(pedidoJson);

                var pedidoJson = session.Metadata["pedido"];
                var pedido = JsonSerializer.Deserialize<Pedido>(pedidoJson);



                Console.WriteLine("Pagamento confirmado!");

                var resultado = await _service.AdicionarPedido(pedido);

                if (!resultado)
                {
                    return BadRequest("Erro ao confirmar pedido");
                }

                await _hub.Clients
                    .Group($"loja")
                    .SendAsync(
                        "ReceiveMessage",
                        pedido
                    );

                await _hub.Clients
                    .Group($"{pedido.ContatoCliente}")
                    .SendAsync(
                        "ReceiveMessage",
                        $"Pedido: {pedido.Id} confirmado!"
                    );

            }

            // PaymentIntent pago
            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                Console.WriteLine($"Pagamento aprovado: {paymentIntent.Id}");
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest($"Webhook Error: {ex.Message}");
        }
    }
}
