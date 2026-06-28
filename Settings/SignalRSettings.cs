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


    // ARMAZENA CONEXÕES
    public class WebSocket
    {
        public List<Conexao> User { get; set; } = new();
    }


    // CONFIGURAÇÕES
    public class SignalRSettings : Hub
    {
        private readonly IPedidoService _service;
        private readonly HttpClient _httpClient;
        private readonly WebSocket _conn;

        public SignalRSettings(IPedidoService service, IHttpClientFactory httpClientFactory, WebSocket connection)
        {
            _service = service;
            _httpClient = httpClientFactory.CreateClient("apiPDF");
            _conn = connection;
        }

        // USUÁRIO CONECTADO
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Usuário conectado: {Context.ConnectionId}");

            await base.OnConnectedAsync();
        }

        // USUÁRIO DESCONECTADO
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Usuário desconectado: {Context.ConnectionId}");

            //RETORNA CONEXAO COM ID EQUIVALENTE
            Conexao? con = _conn.User.FirstOrDefault(u => u.id == Context.ConnectionId);

            //VERIFICA SE CONEXÃO EXISTE NA LISTA

            if (con != null)
            {
                _conn.User.Remove(con);
                Console.WriteLine($"Usuário {con.id} saiu da sala {con.sala}");
            }

            await base.OnDisconnectedAsync(exception);

        }

        // ENTRAR NA SALA
        public async Task EntrarSala(string sala)
        {
            //ADICIONAR VERIFICAÇÃO PARA ENTRAR NA SALA 'loja'
            await Groups.AddToGroupAsync(Context.ConnectionId, sala);

            Console.WriteLine($"{Context.ConnectionId} entrou na sala: {sala}");

            //CONEXAO COM ID E NOME SALA
            var con = new Conexao{id = Context.ConnectionId, sala = sala};

            //VERIFIQUE SE EXISTE ALGUMA CONEXÃO COM O ID ARMAZENADO NO LISTA
            if (!_conn.User.Any(c => c.id == Context.ConnectionId))
            {
                _conn.User.Add(con);
            }
        }

        // SAIR DA SALA
        public async Task SairSala(string sala)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, sala);
            Console.WriteLine($"{Context.ConnectionId} saiu da sala: {sala}");

            //RETORNA CONEXAO COM ID EQUIVALENTE
            Conexao? con = _conn.User.FirstOrDefault(u => u.id == Context.ConnectionId);

            if (con != null)
            {
                _conn.User.Remove(con);
            }
        }



        ///////////////////////// RECEBER PEDIDO \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        public async Task CreatePedido(Pedido pedido)
        // *O NOME DESTA FUNÇÃO DEVE SER CHAMADO NO connection.invoke() USADO NO FRONT*
        {

            Console.WriteLine($"____________________ Pedido Recebido _________________________");

            /*
            Console.WriteLine(
                JsonSerializer.Serialize(pedido, new JsonSerializerOptions{WriteIndented = true})
            );
            */

            Console.WriteLine($"ID do pedido: {pedido.Id}");
            Console.WriteLine("________________________________________________________");


            // ENVIAR PARA TODOS
            //await Clients.All.SendAsync("ReceiveMessage", pedido);

            try
            {
                foreach(var sala in _conn.User)
                {
                    if (sala.sala == "loja")
                    {
                        Console.WriteLine("Loja Online");

                        var confirm = await this._service.AdicionarPedido(pedido);
                        if (!confirm) throw new Exception("Erro ao registrar pedido");

                        // ENVIA SOMENTE PARA LOJA
                        await Clients.Group("loja").SendAsync("ReceiveMessage", pedido);

                        //ENVIA PARA CLIENTE
                        await Clients.Group($"{pedido.ContatoCliente}").SendAsync("ReceiveMessage", "Aguardando confirmação...");

                        return;

                    }

                }

                Console.WriteLine("Loja Offline");

                await Clients.Group($"{pedido.ContatoCliente}")
                        .SendAsync("ReceiveMessage", "Loja offline");

            }
            catch(Exception er)
            {
                Console.WriteLine($"Excpetion: {er}");
                await Clients.Group($"{pedido.ContatoCliente}").SendAsync("ReceiveMessage", $"{er.Message}");
            }

        }

    }
}
