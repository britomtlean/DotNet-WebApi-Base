using MongoDB.Driver;
using WebApi2026.Interfaces;
using WebApi2026.Context;
using WebApi2026.Entities;
using WebApi2026.Settings;

namespace WebApi2026.Services
{
    public class ProdutosService : IProdutosService
    {
        private readonly IMongoCollection<Produto> _produtosCollection;
        private readonly FilesSettings _files;

        private readonly CloudinarySettings _cloudinary;

        public ProdutosService(AppDbContext context, FilesSettings files, CloudinarySettings cloudinary)
        {
            _produtosCollection = context.Produto;
            _files = files;
            _cloudinary = cloudinary;
        }

        public async Task<List<Produto>> AddProduct(Produto produto, IFormFile arquivo)
        {

            // UPLOAD DE ARQUIVOS NO SERVIDOR
            /*
            if(arquivo == null || produto == null)
            {
                throw new Exception("Dados inválidos");
            }

            var diretorioImagem = await _files.Download(arquivo);
            */

            var diretorioImagem = await _cloudinary.UploadImageAsync(arquivo);

            await _produtosCollection.InsertOneAsync(
                new Produto
                {
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    Valor = produto.Valor,
                    Categoria = produto.Categoria,
                    Estoque = produto.Estoque,
                    Imagem = diretorioImagem
                }
            );

            var produtos = await _produtosCollection
                .Find(_ => true)
                .ToListAsync();

                return produtos;
        }

        public  Task<List<Produto>> ReturnProducts()
        {
            var produtos = this._produtosCollection.Find(_ => true).ToListAsync();

            return produtos;
        }

        public async Task<Object> UpdateProduct(string id, Produto update)
        {
            var produto = await _produtosCollection.Find(p => p.Id == id)
                                       .FirstOrDefaultAsync();

            if (produto == null)
                return "Produto não encontrado";

            await this._produtosCollection.UpdateOneAsync(
                p => p.Id == id,
                Builders<Produto>.Update.Set(p => p.Disponibilidade, update.Disponibilidade )
            );

            await this._produtosCollection.UpdateOneAsync(
                p => p.Id == id,
                Builders<Produto>.Update.Set(p => p.Nome, update.Nome)
            );

            await this._produtosCollection.UpdateOneAsync(
                p => p.Id == id,
                Builders<Produto>.Update.Set(p => p.Valor, update.Valor)
            );

            produto = await _produtosCollection.Find(p => p.Id == id)
                                       .FirstOrDefaultAsync();

            return new {produto};
        }

        public async Task<bool> DeleteProduct(string id)
        {
            await _produtosCollection.DeleteOneAsync(p => p.Id == id);

            return true;
        }
    }
}
