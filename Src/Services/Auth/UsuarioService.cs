using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using MongoDB.Driver;
using WebApi2026.Context;
using WebApi2026.Entities;
using WebApi2026.Interfaces;

namespace WebApi2026.Services
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IMongoCollection<Usuario> _usuarios;
        // Tabela Usuarios

        public UsuarioService(AppDbContext context)
        {
            _usuarios = context.Usuarios;
        }

        public async Task<Usuario?> GetUnique(string login)
        {
            return await _usuarios.Find(u => u.User == login).FirstOrDefaultAsync();
        }

        /////

        public async Task<bool> UpdateUser(string login, Usuario dados)
        {

            var user = await _usuarios.Find(u => u.User == login).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            var updateDefinition = Builders<Usuario>.Update
                .Set(u => u.Descricao, dados.Descricao)
                .Set(u => u.Endereco, dados.Endereco)
                .Set(u => u.Horario, dados.Horario)
                .Set(u => u.Instagram, dados.Instagram)
                .Set(u => u.WhatsApp, dados.WhatsApp);

            await _usuarios.UpdateOneAsync(
                u => u.User == login,
                updateDefinition
            );
            return true;
        }
    }
}
