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

        public async Task<Usuario?> GetUnique(string cpf)
        {
            return await _usuarios.Find(u => u.Cpf == cpf).FirstOrDefaultAsync();
        }

    }
}
