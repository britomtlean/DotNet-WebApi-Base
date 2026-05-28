using Microsoft.AspNetCore.SignalR;
using WebApi2026.Entities;
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

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;

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

        // ENTRAR NA SALA
        public async Task EntrarSala(string sala)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, sala);

            Console.WriteLine($"{Context.ConnectionId} entrou na sala: {sala}");
        }

        // SAIR DA SALA
        public async Task SairSala(string sala)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, sala);
        }



        // RECEBER PEDIDO
        public async Task CreatePedido(Pedido pedido)
        // *O NOME DESTA FUNÇÃO DEVE SER CHAMADO NO connection.invoke() USADO NO FRONT*
        {
            Console.WriteLine("____________________ Pedido _________________________");
            Console.WriteLine(
                JsonSerializer.Serialize(
                    pedido,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }
                )
            );
            Console.WriteLine("________________________________________________________");

            /*
                _chatService.Messages.Add();
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
            */

            // ENVIAR PARA TODOS
            //await Clients.All.SendAsync("ReceiveMessage", pedido);

            // ENVIA SOMENTE PARA LOJA
            await Clients.Group("loja")
                .SendAsync("ReceiveMessage", pedido);

            await Clients.Group($"{pedido.ContatoCliente}")
                .SendAsync("ReceiveMessage", "Pedido enviado, aguarde a confirmação.");

            //}
        }

    }
}
