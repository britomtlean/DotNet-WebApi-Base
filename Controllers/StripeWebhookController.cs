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
    private readonly string _endpointSecret;
    private readonly IPedidoService _service;
    private readonly IHubContext<SignalRSettings> _hub;


    public StripeWebhookController(IPedidoService service, IHubContext<SignalRSettings> hub, IConfiguration configuration)
    {
        _service = service;
        _hub = hub;
        _endpointSecret = configuration["Stripe:WebhookSecret"];
    }

    [HttpPost]
    public async Task<IActionResult> StripeWebHook()
    {
        Console.WriteLine($"____________________ WEBHOOK RECEBIDO _________________________");

        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                _endpointSecret
            );

            ////////////////// PAGAMENTO INICIADO \\\\\\\\\\\\\\\\\\\\\\

            if (stripeEvent.Type == "payment_intent.created")
            {
                Console.WriteLine("payment_intent.created ok");
                return Ok();
            }

            ////////////////////////////////////////////////////////////

            ////////////////// COBRANÇA REALIZADA \\\\\\\\\\\\\\\\\\\\\\

            if (stripeEvent.Type == "charge.succeeded")
            {
                Console.WriteLine("charge.succeeded Ok");
                return Ok();
            }

            ///////////////////////////////////////////////////////////

            ////////////////// PAGAMENTO CONCLUIDO \\\\\\\\\\\\\\\\\\\\\\

            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                Console.WriteLine("Pagamento confirmado!");

                var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                var dadosPedido = paymentIntent.Metadata["pedido"];

                Pedido pedido = await this._service.PedidoId(dadosPedido);

                if(pedido == null)
                {
                    throw new Exception($"Erro ao processar pedido {dadosPedido}");
                }

                Console.WriteLine("Dados recebidos confirmados");

                var resultado = await _service.ConfirmarPedido(pedido);
                pedido = await this._service.PedidoId(dadosPedido);
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

            ////////////////// CHECKOUT CONCLUIDO \\\\\\\\\\\\\\\\\\\\\\

            if (stripeEvent.Type == "checkout.session.completed")
            {
                Console.WriteLine("checkout concluído");
                return Ok();
            }

            ////////////////////////////////////////////////////////////


            return BadRequest("Pagamento não aprovado");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return BadRequest($"Webhook Error: {ex.Message}");
        }
    }
}
