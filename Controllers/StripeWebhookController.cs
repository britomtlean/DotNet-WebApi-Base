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
    private readonly IHubContext<SignalRSettings> _hub;


    public StripeWebhookController(IPedidoService service, IHubContext<SignalRSettings> hub)
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

            ////////////////// PAGAMENTO CONCLUIDO \\\\\\\\\\\\\\\\\\\\\\

            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                Console.WriteLine("Pagamento confirmado!");

                var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                var dadosPedido = paymentIntent.Metadata["pedido"];
                Console.WriteLine("Dados recebidos");

                Pedido pedido = await this._service.PedidoId(dadosPedido);
                Console.WriteLine("Dados confirmados");

                var resultado = await _service.ConfirmarPedido(pedido);
                pedido = await this._service.PedidoId(dadosPedido);
                Console.WriteLine(pedido.Status);
                Console.WriteLine("Pedido confirmado");

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

                return Ok();
            }

            //////////////////////////////////////////////////////////////

            return BadRequest("Pagamento não aprovado");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return BadRequest($"Webhook Error: {ex.Message}");
        }
    }
}
