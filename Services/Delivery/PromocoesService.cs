using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using WebApi2026.Context;
using WebApi2026.Entities;
using WebApi2026.Interfaces;
using WebApi2026.Settings;

namespace WebApi2026.Services
{
    public class PromocoesService : IPromocoesService
    {
        private readonly IMongoCollection<Promocoes> _promocoesCollection;

        private readonly CloudinarySettings _cloudinary;

        public PromocoesService(AppDbContext context, CloudinarySettings cloudinary)
        {
            _promocoesCollection = context.Promocoes;
            _cloudinary = cloudinary;
        }


        public async Task<Promocoes?> CriarPromocao(IFormFile file, Promocoes promocao)
        {
            var diretorioImagem = await _cloudinary.UploadImageAsync(file);

            promocao.Imagem = diretorioImagem;

            await _promocoesCollection.InsertOneAsync(promocao);;

            var promocaoCriada = await _promocoesCollection
                .Find(p => p.Login == promocao.Login)
                .FirstOrDefaultAsync();

            return promocaoCriada;

        }

        public async Task<List<Promocoes>?> ReturnPromocoes(string login)
        {
            var promocoes = await this._promocoesCollection.Find(p => p.Login == login).ToListAsync();

            return promocoes;
        }


    }
}
