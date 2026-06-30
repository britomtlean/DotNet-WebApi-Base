using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApi2026.Interfaces; //Interface
using MongoDB.Driver; //Mongo
using WebApi2026.Entities; //Entitie
using WebApi2026.Context; //Context

using WebApi2026.Settings; //Token
using WebApi2026.Types; //Type

namespace WebApi2026.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMongoCollection<Usuario> _usuario;
        private readonly TokenSettings _tokenSettings;

        public AuthService(AppDbContext context, TokenSettings tokenSettings) //Construtor
        {
            _usuario = context.Usuarios;
            _tokenSettings = tokenSettings;
        }

        /////////////////////// FUNCTIONS \\\\\\\\\\\\\\\\\\\\\\\\\\

        public async Task<string> Login(Login login)
        {
            Console.WriteLine("SOLICITAÇÃO DE LOGIN:");
            Console.WriteLine($"CPF: {login.Cpf}");

            var loginTrue = await _usuario.Find(user => user.Cpf == login.Cpf).FirstOrDefaultAsync();


            if (loginTrue == null)
            {
                throw new Exception("Cpf não encontrado");
            }

            if (loginTrue.Senha != login.Password)
            {
                throw new Exception("Senha incorreta!");
            }

            var token = _tokenSettings.GerarToken(loginTrue.Cpf);
            Console.WriteLine($"USUÁRIO {login.Cpf} AUTENTICADO COM SUCESSO");
            return token;
        }


        public async Task<Object> Register(Usuario newUser)
        {

            Console.WriteLine("Dados recebidos:");
            Console.WriteLine($"CPF:{newUser.Cpf}");
            Console.WriteLine($"Nome:{newUser.Nome}");
            Console.WriteLine($"Senha:{newUser.Senha}");

            if (newUser == null)
            {
                throw new Exception("Dados inválidos");
            }

            // Verificar se usuário existe
            var usuarioExistente = await _usuario.Find(u => u.Cpf == newUser.Cpf).FirstOrDefaultAsync();

            if (usuarioExistente != null)
            {
                throw new Exception("CPF já cadastrado");
            }

            await _usuario.InsertOneAsync(newUser);

            return new
            {
                mensagem = "Usuario criado com sucesso",
                id = newUser.Id
            };
        }
    }
}
