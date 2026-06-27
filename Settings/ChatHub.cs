using Microsoft.AspNetCore.SignalR;
using WebApi2026.Entities;
using System.Text.Json;
using WebApi2026.Services;
using WebApi2026.Interfaces;
using System.Net.Http;

namespace WebApi2026.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IPedidoService _service;
        private readonly HttpClient _httpClient;


        public ChatHub(IPedidoService service, IHttpClientFactory httpClientFactory)
        {
            _service = service;
            _httpClient = httpClientFactory.CreateClient("apiPDF");
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

            Console.WriteLine($"____________________ Pedido Recebido _________________________");

            Console.WriteLine(
                JsonSerializer.Serialize(pedido, new JsonSerializerOptions{WriteIndented = true})
            );
            Console.WriteLine("________________________________________________________");


            // ENVIAR PARA TODOS
            //await Clients.All.SendAsync("ReceiveMessage", pedido);

            try
            {
                var confirm = await this._service.AdicionarPedido(pedido);

                if(confirm == true)
                {
                    var res = await _httpClient.PostAsJsonAsync("api/produtos", pedido);

                    if (!res.IsSuccessStatusCode)
                    {
                        Console.WriteLine(await res.Content.ReadAsStringAsync());
                        return;
                    }
                    
                    // ENVIA SOMENTE PARA LOJA
                    await Clients.Group("loja")
                        .SendAsync("ReceiveMessage", pedido);

                    //ENVIA PARA CLIENTE
                    await Clients.Group($"{pedido.ContatoCliente}")
                        .SendAsync("ReceiveMessage", "Aguardando confirmação...");


                }
            }
            catch(Exception er)
            {
                await Clients.Group($"{pedido.ContatoCliente}")
                .SendAsync("ReceiveMessage", $"{er.Message}");
            }

        }

    }
}
