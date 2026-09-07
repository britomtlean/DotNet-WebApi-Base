using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;

using WebApi2026.Interfaces;
using MongoDB.Driver;
using WebApi2026.Entities;
using WebApi2026.Context;
using WebApi2026.Settings;

namespace WebApi2026.Services
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IMongoCollection<Usuario> _usuarios;
        private readonly CloudinarySettings _cloudnary;


        public UsuarioService(AppDbContext context, CloudinarySettings cloudnary)
        {
            _usuarios = context.Usuarios;
            _cloudnary = cloudnary;
        }

        public async Task<Usuario?> GetUnique(string login)
        {
            return await _usuarios.Find(u => u.User == login).FirstOrDefaultAsync();
        }

        public async Task<Usuario?> GetForLogin(string login)
        {
            var usuario = await _usuarios.Find(u => u.User == login).FirstOrDefaultAsync();

            return usuario;
        }

        public async Task<bool> Update(string login, Usuario dados, IFormFile file)
        {

            var imageName = await _cloudnary.UploadImageAsync(file);

            var updateDefinition = Builders<Usuario>.Update
                .Set(u => u.Descricao, dados.Descricao)
                .Set(u => u.Endereco, dados.Endereco)
                .Set(u => u.Horario, dados.Horario)
                .Set(u => u.Instagram, dados.Instagram)
                .Set(u => u.WhatsApp, dados.WhatsApp)
                .Set(u => u.Logo, imageName);

            await _usuarios.UpdateOneAsync(
                u => u.User == login,
                updateDefinition
            );

            return true;
        }
    }
}
