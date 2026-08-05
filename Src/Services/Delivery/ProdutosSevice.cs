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

        public async Task<List<Produto>> AddProduct(Produto produto, IFormFile arquivo, string cpf)
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
                    Cpf = cpf,
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

        public async Task<Object> UpdateProduct(string id, Produto update, string cpf)
        {
            var produto = await _produtosCollection.Find(p => p.Id == id && p.Cpf == cpf)
                                       .FirstOrDefaultAsync();

            if (produto == null)
                return "Produto não encontrado";

            var updateDefinition = Builders<Produto>.Update
                .Set(p => p.Disponibilidade, update.Disponibilidade)
                .Set(p => p.Nome, update.Nome)
                .Set(p => p.Descricao, update.Descricao)
                .Set(p => p.Valor, update.Valor);

            await _produtosCollection.UpdateOneAsync(
                p => p.Id == id && p.Cpf == cpf,
                updateDefinition
            );

            produto = await _produtosCollection.Find(p => p.Id == id && p.Cpf == cpf)
                                       .FirstOrDefaultAsync();

            return new {produto};
        }

        public async Task<bool> DeleteProduct(string id, string cpf)
        {
            await _produtosCollection.DeleteOneAsync(p => p.Id == id && p.Cpf == cpf);

            return true;
        }
    }
}
