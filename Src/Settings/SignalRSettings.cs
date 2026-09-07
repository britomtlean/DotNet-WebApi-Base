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


    public class DadosSala
    {
        public string sala { get; set; }
        public string? chaveAcesso { get; set; }
    }

    // CONFIGURAÇÕES
    public class SignalRSettings : Hub
    {
        private readonly IPedidoService _service;
        private readonly HttpClient _httpClient;
        private readonly WebSocket _conn;

        private readonly IUsuarioService _serviceUser;

        public SignalRSettings(IPedidoService service, IHttpClientFactory httpClientFactory, WebSocket connection, IUsuarioService serviceUser)
        {
            _service = service;
            _httpClient = httpClientFactory.CreateClient("apiPDF");
            _conn = connection;
            _serviceUser = serviceUser;
        }

        // USUÁRIO CONECTADO
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Usuário conectado: {Context.ConnectionId}");

            await base.OnConnectedAsync();
        }



        // ENTRAR NA SALA
        public async Task EntrarSala(string req)
        {
            try
            {
                DadosSala? dados;

                /////////////////// VALIDAÇÃO DE REQUISIÇÃO ////////////////////////////////

                using (JsonDocument json = JsonDocument.Parse(req))
                {
                    if (json.RootElement.ValueKind == JsonValueKind.String)
                    {
                        // "loja"
                        dados = new DadosSala
                        {
                            sala = json.RootElement.GetString()!,
                            chaveAcesso = null
                        };
                    }
                    else if (json.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        // { "sala": "loja", "chaveAcesso": "delivery1234" }
                        dados = JsonSerializer.Deserialize<DadosSala>(req);
                    }
                    else
                    {
                        await Clients.Caller.SendAsync(
                        "Erro",
                        "Formato inválido na requisição.");

                        Console.WriteLine("Formato inválido na requisição. Operação cancelada.");
                        return;
                    }
                }

                /////////////////////////////////////////////////////////////


                ///////////////////////////////////////////////////////////////////

                if (dados == null || string.IsNullOrWhiteSpace(dados.sala))
                {
                    await Clients.Caller.SendAsync(
                        "Erro",
                        "Dados na requisição inválidos."
                    );

                    Console.WriteLine("Dados na requisição inválidos. Operação cancelada.");
                    return;
                }

                ///////////////////////////////////////////////////////////////////


                // Cria instancia de conexção
                var con = new Conexao { id = Context.ConnectionId, sala = dados.sala };

                ////////////////////////////////////////////////////////////////

                // Verifica se o id recebido já possui alguma conexão
                if (_conn.User.Any(c => c.id == con.id))
                {
                    await Clients.Caller.SendAsync(
                        "Erro",
                        "Esta sessão ja possui uma conexão. Sua conexão será interrompida."
                    );

                    Context.Abort();

                    Console.WriteLine("Esta sessão ja possui uma conexão. Operação cancelada.");
                    return;
                }

                //////////////////////////////////////////////////////////////


                ////////////////////////////////////////////////////////////

                var loja = await _serviceUser.GetForLogin(dados.sala);

                if (loja != null)
                {
                    if (dados.sala == loja.User)
                    {
                        if (dados.chaveAcesso != "delivery1234")
                        {
                            await Clients.Caller.SendAsync(
                                "Erro",
                                "Chave de acesso inválida."
                            );

                            Console.WriteLine("Chave de acesso inválida. Operação cancelada.");
                            return;
                        }


                        // Conclui conexão
                        await Groups.AddToGroupAsync(con.id, con.sala);
                        _conn.User.Add(con);
                        Console.WriteLine($"{Context.ConnectionId} entrou na sala: loja");

                        await Clients.Caller.SendAsync(
                            "Conectado",
                            "Conexão bem sucedida."
                        );

                        return;
                    }
                }


                //////////////////////////////////////////////////////////

                // Conclui conexão
                await Groups.AddToGroupAsync(con.id, con.sala);
                _conn.User.Add(con);
                Console.WriteLine($"{Context.ConnectionId} entrou na sala: {dados.sala}");
            }
            catch (JsonException)
            {
                await Clients.Caller.SendAsync(
                    "Erro",
                    "JSON inválido."
                );

                Console.WriteLine("Erro ao converter JSON.");
            }
            catch (Exception er)
            {
                Console.WriteLine(er);

                await Clients.Caller.SendAsync(
                    "Erro",
                    "Erro ao realizar conexão"
                );
            }
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
                await Groups.RemoveFromGroupAsync(con.id, con.sala);
                Console.WriteLine($"Usuário {con.id} saiu da sala {con.sala}");
            }

            await base.OnDisconnectedAsync(exception);

        }

        // SAIR DA SALA
        public async Task SairSala(string sala)
        {

            //RETORNA CONEXAO COM ID EQUIVALENTE
            Conexao? con = _conn.User.FirstOrDefault(u => u.id == Context.ConnectionId);

            if (con == null)
            {
                await Clients.Caller.SendAsync(
                "Erro",
                "Você não está em nenhuma sala."
            );
                return;
            }

            _conn.User.Remove(con);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, con.sala);
            Console.WriteLine($"{Context.ConnectionId} saiu da sala: {con.sala}");
        }



        ///////////////////////// RECEBER PEDIDO \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        public async Task CreatePedido(Pedido pedido)
        // *O NOME DESTA FUNÇÃO DEVE SER CHAMADO NO connection.invoke() USADO NO FRONT*
        {

            Console.WriteLine($"____________________ Pedido Recebido _________________________");

            Console.WriteLine($"ID do pedido: {pedido.Id}");
            Console.WriteLine("________________________________________________________");


            try
            {
                if (string.IsNullOrWhiteSpace(pedido.ContatoCliente) || string.IsNullOrWhiteSpace(pedido.Loja))
                {
                    throw new Exception("Dados inválidos");
                }

                var estabelecimento = await _serviceUser.GetForLogin(pedido.Loja);

                if (estabelecimento == null)
                {
                    Console.WriteLine("Estabelecimento não encontrado");
                    await Clients.Group($"{pedido.ContatoCliente}").SendAsync("ReceiveMessage", "Estabelecimento não encontrado");
                    return;
                }

                // Verifica se a loja está online
                var lojaOnline = _conn.User.Any(x => x.sala == pedido.Loja);

                if (!lojaOnline)
                {
                    Console.WriteLine("Status da Loja: Offline");

                    await Clients.Group(pedido.ContatoCliente)
                        .SendAsync("ReceiveMessage", "Loja offline");

                    Console.WriteLine("Pedido cancelado");
                    return;
                }


                Console.WriteLine("Loja Online");

                await this._service.AdicionarPedido(pedido);
                Console.WriteLine($"Pedido {pedido.Id} gerado com sucesso");

                // ENVIA SOMENTE PARA LOJA
                await Clients.Group(pedido.Loja).SendAsync("ReceiveMessage", pedido);

                //ENVIA PARA CLIENTE
                await Clients.Group($"{pedido.ContatoCliente}").SendAsync("ReceiveMessage", "Aguardando confirmação...");

                Console.WriteLine("Pedido enviado");

            }
            catch(Exception er)
            {
                Console.WriteLine($"Excpetion: {er}");
            }

        }

    }
}
