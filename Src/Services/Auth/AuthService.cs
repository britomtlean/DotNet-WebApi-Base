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
            Console.WriteLine($"Usuario: {login.User}");

            var loginTrue = await _usuario.Find(loginDb => loginDb.User == login.User).FirstOrDefaultAsync();


            if (loginTrue == null)
            {
                throw new Exception("Usuario não encontrado");
            }

            if (loginTrue.Senha != login.Password)
            {
                throw new Exception("Senha incorreta!");
            }

            var token = _tokenSettings.GerarToken(loginTrue.User);
            Console.WriteLine($"USUÁRIO {login.User} AUTENTICADO");
            return token;
        }


        public async Task<Object> Register(Usuario newLogin)
        {

            Console.WriteLine("Dados recebidos:");
            Console.WriteLine($"CPF:{newLogin.User}");
            Console.WriteLine($"Nome:{newLogin.Nome}");
            Console.WriteLine($"Senha:{newLogin.Senha}");

            if (newLogin == null)
            {
                throw new Exception("Dados inválidos");
            }

            // Verificar se usuário existe
            var usuarioExistente = await _usuario.Find(loginDb => loginDb.User == newLogin.User).FirstOrDefaultAsync();

            if (usuarioExistente != null)
            {
                throw new Exception("Usuario já cadastrado");
            }

            await _usuario.InsertOneAsync(newLogin);

            return new
            {
                mensagem = "Usuario criado com sucesso",
                usuario = newLogin.User
            };
        }
    }
}
