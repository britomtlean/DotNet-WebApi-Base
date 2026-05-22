using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApi2026.Interfaces;
using MongoDB.Driver;
using WebApi2026.Entities;
using WebApi2026.Context;

namespace WebApi2026.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IMongoCollection<Pedido> _pedido;
        private readonly IMongoCollection<Produto> _produto;

        public PedidoService(AppDbContext context)
        {
            _pedido = context.Pedido;
            _produto = context.Produto;
        }

        public async Task<Boolean> AdicionarPedido(Pedido pedido)
        {
            //Validação de produtos
            foreach (var array in pedido.Produtos)
            {
                var produtoExiste = await this._produto.Find(produto => produto.Id == array.ProdutoId).FirstOrDefaultAsync();

                if (produtoExiste == null)
                {
                    throw new Exception($"Produto {array.ProdutoId} não encontrado");
                }

            }

            /*

            .map()

            var arrayIdProdutos = pedido.Produtos.Select(produto => produto.ProdutoId).ToList();

            var produtosVerificados = await _produtos.Find(p => arrayIdProdutos.Contains(p.Id)).ToListAsync();

            if (produtosVerificados.Count != arrayIdProdutos.Count)
            {
                throw new Exception("Um ou mais produtos não existem");
            }

            */


            await this._pedido.InsertOneAsync
            (
                new Pedido
                {
                    Produtos = pedido.Produtos,
                    //ValorTotal = pedido.Produtos.Sum(p => p.Subtotal),
                    ValorTotal = pedido.ValorTotal,
                    NomeCliente = "",
                    ContatoCliente = "",
                    EnderecoCliente = ""
                }
            );

            return true;
        }

    }
}
