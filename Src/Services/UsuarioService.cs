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



        public async Task Create(Usuario usuario)
        {
            await _usuarios.InsertOneAsync(usuario);
        }


        public async Task<List<Usuario>> GetAll()
        {
            return await _usuarios.Find(_ => true).ToListAsync();
        }


        public async Task<Usuario?> GetUnique(string cpf)
        {
            return await _usuarios.Find(u => u.Cpf == cpf).FirstOrDefaultAsync();
        }


        public async Task<Object> Delete(string id)
        {
            await _usuarios.DeleteOneAsync(u => u.Id == id);
            return new
            {
                sucesso = true,
                mensagem = "Usuário deletado com sucesso"
            };
        }


        public async Task UpdateNome(string id, string novoNome)
        {
            await _usuarios.UpdateOneAsync(
                u => u.Id == id,
                Builders<Usuario>.Update.Set(u => u.Nome, novoNome)
            );
        }
    }
}
