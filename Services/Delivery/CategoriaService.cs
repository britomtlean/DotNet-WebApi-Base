using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using MongoDB.Driver;
using WebApi2026.Context;
using WebApi2026.Entities;
using WebApi2026.Interfaces;
using WebApi2026.Settings;

namespace WebApi2026.Services
{
    public class CategoriaService: ICategoriaService
    {
        private readonly IMongoCollection<Categoria> _collectionCategoria;
        private readonly CloudinarySettings _cloudinary;

        public CategoriaService(AppDbContext context, CloudinarySettings cloudinary)
        {
            _collectionCategoria = context.categoria;
            _cloudinary = cloudinary;
        }

        public async Task<string> Create(string login, Categoria data, IFormFile file)
        {

            var fileName = await _cloudinary.UploadImageAsync(file);

            data.Login = login;
            data.Imagem = fileName;

            await _collectionCategoria.InsertOneAsync(data);

            return "Categoria criada com sucesso";
        }

        public async Task<List<Categoria>?> SelectAll(string login)
        {
            var categorias = await _collectionCategoria.Find(c => c.Login == login).ToListAsync();
            return categorias;
        }
    }
}
