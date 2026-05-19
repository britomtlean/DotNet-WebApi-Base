using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi2026.Interfaces; //Interface
using MongoDB.Driver; //Mongo
using WebApi2026.Entities; //Entitie
using WebApi2026.Settings; //Token
using WebApi2026.Context; //Context
using WebApi2026.Types; //Type

namespace WebApi2026.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMongoCollection<Usuario> _usuarios; //Conexão com a tabela
        // Tabela Usuarios
        private readonly TokenService _tokenService; //Token

        public AuthService(AppDbContext context, TokenService tokenService) //Construtor
        {
            _usuarios = context.Usuarios;
            _tokenService = tokenService;
        }

        public async Task<Object> Login(Login login)
        {
            Console.WriteLine("Dados recebidos:");
            Console.WriteLine($"CPF: {login.Cpf}");
            Console.WriteLine($"Senha: {login.Password}");

            var loginDB = await _usuarios.Find(user => user.Cpf == login.Cpf).FirstOrDefaultAsync();
            //Console.WriteLine($"CPF: {loginDB.Cpf}");
            //Console.WriteLine($"Senha: {loginDB.Senha}");


            if (loginDB == null)
            {
                throw new Exception("Cpf não encontrado");
            }

            if (loginDB.Senha != login.Password)
            {
                throw new Exception("Senha incorreta!");
            }

            var token = _tokenService.GerarToken(loginDB.Cpf);
            return new { token };
        }


        public async Task<Object> Register(Usuario register)
        {
            if (register == null)
            {
                throw new Exception("Dados inválidos");
            }

            var usuarioExistente = await _usuarios.Find(u => u.Cpf == register.Cpf).FirstOrDefaultAsync();

            if (usuarioExistente != null)
            {
                throw new Exception("CPF já cadastrado");
            }


            Console.WriteLine("Dados recebidos:");
            Console.WriteLine($"CPF:{register.Cpf}");
            Console.WriteLine($"Nome:{register.Nome}");
            Console.WriteLine($"Senha:{register.Senha}");

            await _usuarios.InsertOneAsync(register);

            return new
            {
                mensagem = "Usuario criado com sucesso",
                id = register.Id
            };
        }
    }
}
