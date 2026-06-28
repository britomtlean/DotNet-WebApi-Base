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
        private readonly HttpClient _httpClient;

        public PedidoService(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _pedido = context.Pedido;
            _produto = context.Produto;
            _httpClient = httpClientFactory.CreateClient("apiPDF");
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

            await this._pedido.InsertOneAsync(pedido);

            return true;
        }

        public async Task<Boolean> ConfirmarPedido(Pedido pedido)
        {
            await _pedido.UpdateOneAsync(p => p.Id == pedido.Id, Builders<Pedido>.Update.Set(p => p.Status, true));

            var res = await _httpClient.PostAsJsonAsync("gerarPDF", pedido);

            if (!res.IsSuccessStatusCode)
            {
                Console.WriteLine("Erro ao gerar pdf");
                Console.WriteLine(await res.Content.ReadAsStringAsync());
            }

            return true;
        }

        public async Task<Boolean> CancelarPedido(Pedido pedido)
        {
            var status = await  _pedido.UpdateOneAsync(p => p.Id == pedido.Id, Builders<Pedido>.Update.Set(p => p.Status, false));
            return false;
        }

        public async Task<List<Pedido>> RetornarPedido()
        {
            var pedidos = await _pedido.Find(_ => true).ToListAsync();

            return pedidos;
        }

    }
}
