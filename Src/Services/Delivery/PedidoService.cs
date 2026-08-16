using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApi2026.Interfaces;
using MongoDB.Driver;
using WebApi2026.Entities;
using WebApi2026.Context;
using System.Runtime.CompilerServices;

namespace WebApi2026.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IMongoCollection<Pedido> _pedido;
        private readonly IMongoCollection<Produto> _produto;

        private readonly IProdutosService _service;

        public PedidoService(AppDbContext context, IProdutosService service)
        {
            _pedido = context.Pedido;
            _produto = context.Produto;
            _service = service;
        }

        public async Task<Boolean> AdicionarPedido(Pedido pedido)
        {
            //Validação de produtos
            foreach (var array in pedido.Produtos)
            {
                var produtoExiste = await this._produto.Find(produto => produto.Id == array.ProdutoId).FirstOrDefaultAsync();

                if (produtoExiste == null || produtoExiste.Estoque < array.Quantidade)
                {
                    throw new Exception($"Produto do pedido indisponível");
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

            await this._pedido.InsertOneAsync(pedido);

            return true;
        }

        public async Task<Pedido> ConfirmarPedido(string id)
        {

            var pedido = await this.PedidoId(id);

            if(pedido.Status == true) throw new Exception("Este pedido ja foi confirmado.");

             await _pedido.UpdateOneAsync(
                p => p.Id == id,
                Builders<Pedido>.Update.Set(p => p.Status, true)
            );
            try
            {
                await _service.SaidaEstoque(pedido.Produtos);
            }
            catch(Exception er)
            {
                await _pedido.UpdateOneAsync(
                p => p.Id == id,
                Builders<Pedido>.Update.Set(p => p.Status, false));

                throw new Exception(er.Message);

            }

            return pedido;
        }

        public async Task<Pedido> CancelarPedido(string id)
        {
            var pedido = await this.PedidoId(id);

            if (pedido.Status == false) throw new Exception("Este pedido ja foi cancelado.");

            if(pedido.Status == true)
            {
                await _service.EntradaEstoque(pedido.Produtos);
            }

            await  _pedido.UpdateOneAsync(p => p.Id == id, Builders<Pedido>.Update.Set(p => p.Status, false));

            return pedido;
        }

        public async Task<List<Pedido>> RetornarPedido()
        {
            var pedidos = await _pedido.Find(_ => true).ToListAsync();

            return pedidos;
        }

        public async Task<Pedido> PedidoId(string id)
        {
            Pedido pedido = await this._pedido.Find(p => p.Id == id).FirstOrDefaultAsync();

            if(pedido == null)
            {
                throw new Exception("Pedido não encontrado");
            }

            return pedido;
        }
    }
}
