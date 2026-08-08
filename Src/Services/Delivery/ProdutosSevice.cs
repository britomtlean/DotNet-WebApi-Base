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

        public async Task<List<Produto>> AddProduct(Produto produto, IFormFile arquivo, string login)
        {

            // UPLOAD DE ARQUIVOS NO SERVIDOR
            /*
            if(arquivo == null || produto == null)
            {
                throw new Exception("Dados inválidos");
            }

            var diretorioImagem = await _files.Download(arquivo);
            */

            string diretorioImagem = await _cloudinary.UploadImageAsync(arquivo);

            await _produtosCollection.InsertOneAsync(
                new Produto
                {
                    Nome = produto.Nome,
                    Login = login,
                    Descricao = produto.Descricao,
                    Categoria = produto.Categoria,
                    Valor = produto.Valor,
                    Estoque = produto.Estoque,
                    Disponibilidade = produto.Disponibilidade,
                    Imagem = diretorioImagem
                }
            );

            var produtos = await _produtosCollection
                .Find(_ => true)
                .ToListAsync();

                return produtos;
        }

        public  Task<List<Produto>?> ReturnProducts(string login)
        {
            var produtos = this._produtosCollection.Find(p => p.Login == login).ToListAsync();

            return produtos;
        }

        public async Task<Produto?> UpdateProduct(string id, Produto update, string login)
        {
            //VEIRIFICA SE EXISTE
            var produtoExiste = await _produtosCollection.Find(p => p.Id == id)
                                       .FirstOrDefaultAsync();

            if (produtoExiste == null) throw new Exception("Produto não encontrado");

            //VEIRIFICA SE POSSUI VINCULO
            var produtoVinculado = await _produtosCollection.Find(p => p.Id == id && p.Login == login)
                                       .FirstOrDefaultAsync();

            if (produtoVinculado == null) throw new Exception("Você não possui autorização para fazer alterações nesse produto");

            //VALIDAÇÃO BEM SUCEDIDA

            var updateDefinition = Builders<Produto>.Update
                .Set(p => p.Disponibilidade, update.Disponibilidade)
                .Set(p => p.Nome, update.Nome)
                .Set(p => p.Descricao, update.Descricao)
                .Set(p => p.Valor, update.Valor);

            await _produtosCollection.UpdateOneAsync(
                p => p.Id == id && p.Login == login,
                updateDefinition
            );

            produtoVinculado = await _produtosCollection.Find(p => p.Id == id && p.Login == login)
                                       .FirstOrDefaultAsync();

            return produtoVinculado;
        }

        public async Task<string> DeleteProduct(string id, string login)
        {

            //VEIRIFICA SE EXISTE
            var produtoExiste = await _produtosCollection.Find(p => p.Id == id)
                                       .FirstOrDefaultAsync();

            if (produtoExiste == null) throw new Exception("Produto não encontrado");

            //VEIRIFICA SE POSSUI VINCULO
            var produtoVinculado = await _produtosCollection.Find(p => p.Id == id && p.Login == login)
                                       .FirstOrDefaultAsync();

            if (produtoVinculado == null) throw new Exception("Você não possui autorização para fazer alterações nesse produto");


            //VALIDAÇÃO BEM SUCEDIDA

            await _produtosCollection.DeleteOneAsync(p => p.Id == id && p.Login == login);

            return "Produto deletado com sucesso";
        }
    }
}
