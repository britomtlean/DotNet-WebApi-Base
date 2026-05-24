using Microsoft.AspNetCore.SignalR;
using WebApi2026.Entities;
using WebApi2026.Interfaces;
using System.Text.Json;

namespace WebApi2026.Hubs
{

    public class ChatService
    {
        public List<Pedido> Messages { get; set; } = new();
    }


    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;

        private readonly IPedidoService _service;

        public ChatHub(ChatService chatService, IPedidoService service)
        {
            _chatService = chatService;
            _service = service;
        }

        // Usuário conectou
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Usuário conectado: {Context.ConnectionId}");

            await base.OnConnectedAsync();
        }

        // Usuário desconectou
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Usuário desconectado: {Context.ConnectionId}");

            await base.OnDisconnectedAsync(exception);
        }

        // MENSAGEM GLOBAL
        public async Task CreatePedido(Pedido pedido)
        // *O NOME DESTA FUNÇÃO DEVE SER CHAMADO NO connection.invoke() USADO NO FRONT*
        {
            Console.WriteLine($"📩 Mensagem global recebida: {JsonSerializer.Serialize(pedido)}");

            var resultado = await _service.AdicionarPedido(pedido);

            if(resultado)
            {
                _chatService.Messages.Add(pedido);

                // LISTAR ARRAY DE PEDIDOS
                foreach (var item in _chatService.Messages)
                {
                    Console.WriteLine("____________________ Pedido _________________________");
                    Console.WriteLine(
                        JsonSerializer.Serialize(
                            item,
                            new JsonSerializerOptions
                            {
                                WriteIndented = true
                            }
                        )
                    );
                    Console.WriteLine("________________________________________________________");
                }

                await Clients.All.SendAsync("ReceiveMessage", pedido);
            }
        }
    }
}
