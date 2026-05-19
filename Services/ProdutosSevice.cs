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
        private readonly Files _files;

        public ProdutosService(AppDbContext context, Files files)
        {
            _produtosCollection = context.Produto;
            _files = files;
        }

        public async Task<List<Produto>> AddProduct(Produto produto, IFormFile arquivo)
        {
            var diretorioImagem = await _files.Download(arquivo);

            await _produtosCollection.InsertOneAsync(
                new Produto
                {
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    Valor = produto.Valor,
                    Estoque = produto.Estoque,
                    Imagem = diretorioImagem
                }
            );

            return await _produtosCollection
                .Find(_ => true)
                .ToListAsync();
        }
    }
}
