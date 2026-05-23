using Microsoft.AspNetCore.SignalR;

namespace WebApi2026.Hubs
{
    public class ChatHub : Hub
    {
        // Cliente enviando mensagem
        public async Task EnviarMensagem(object data)
        {
            Console.WriteLine($"Pedido recebido: {data}");

            // Envia para todos os clientes conectados
            await Clients.All.SendAsync("mensagem_retorno", data);
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
    }
}
