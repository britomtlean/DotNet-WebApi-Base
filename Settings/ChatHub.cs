using Microsoft.AspNetCore.SignalR;
using WebApi2026.Entities;
using System.Text.Json;
using WebApi2026.Services;
using WebApi2026.Interfaces;
using System.Net.Http;

namespace WebApi2026.Hubs
{

    public class Conexao
    {
        public string id { get; set; } = null!;
        public string sala { get; set; } = null!;
    }
    public class SalaManager
    {
        public List<Conexao> Salas { get; } = new();
    }
    public class ChatHub : Hub
    {
        private readonly IPedidoService _service;
        private readonly HttpClient _httpClient;

        private readonly SalaManager _sala;

        public ChatHub(IPedidoService service, IHttpClientFactory httpClientFactory, SalaManager salaManager)
        {
            _service = service;
            _httpClient = httpClientFactory.CreateClient("apiPDF");
            _sala = salaManager;
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

            //RETORNA CONEXAO COM ID EQUIVALENTE
            Conexao? con = _sala.Salas.FirstOrDefault(c => c.id == Context.ConnectionId);

            //SE CON NÃO EXISTIR RETORNA

            if (con != null)
            {
                _sala.Salas.Remove(con);
            }

            await base.OnDisconnectedAsync(exception);

        }

        // ENTRAR NA SALA
        public async Task EntrarSala(string sala)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, sala);

            Console.WriteLine($"{Context.ConnectionId} entrou na sala: {sala}");

            //CONEXAO COM ID E NOME SALA
            var con = new Conexao{id = Context.ConnectionId, sala = sala};

            //ARMAZENO NO ARRAY
            if (!_sala.Salas.Any(c => c.id == Context.ConnectionId))
            {
                _sala.Salas.Add(con);
            }
        }

        // SAIR DA SALA
        public async Task SairSala(string sala)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, sala);
            Console.WriteLine($"{Context.ConnectionId} saiu da sala: {sala}");

            //RETORNA CONEXAO COM ID EQUIVALENTE
            Conexao? con = _sala.Salas.FirstOrDefault(c => c.id == Context.ConnectionId);

            if (con != null)
            {
                _sala.Salas.Remove(con);
            }
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

                if(!confirm) throw new Exception("Erro ao registrar pedido");

                {
                    /*
                    var res = await _httpClient.PostAsJsonAsync("gerarPDF", pedido);

                    if (!res.IsSuccessStatusCode)
                    {
                        Console.WriteLine(await res.Content.ReadAsStringAsync());
                        return;
                    }
                */

                foreach(var sala in _sala.Salas)
                {
                    if (sala.sala == "loja")
                    {
                        Console.Write("Loja Online");

                        // ENVIA SOMENTE PARA LOJA
                        await Clients.Group("loja")
                        .SendAsync("ReceiveMessage", pedido);

                        //ENVIA PARA CLIENTE
                        await Clients.Group($"{pedido.ContatoCliente}")
                            .SendAsync("ReceiveMessage", "Aguardando confirmação...");

                        return;

                    }

                }

                Console.WriteLine("Loja Offline");

                await Clients.Group($"{pedido.ContatoCliente}")
                        .SendAsync("ReceiveMessage", "Loja offline");

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
