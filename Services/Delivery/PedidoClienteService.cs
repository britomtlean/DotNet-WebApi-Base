using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using WebApi2026.Context;
using WebApi2026.Entities;
using WebApi2026.Interfaces;

namespace WebApi2026.Services
{
    public class PedidoClienteService : IPedidoClienteService
    {

        private readonly IMongoCollection<PedidoCliente> _collection;

        public PedidoClienteService(AppDbContext context)
        {
            _collection = context.pedidoCliente;
        }


        /*********************** SERVICES **************************/

        public async Task<string> Insert(string login, PedidoCliente data)
        {
            await _collection.InsertOneAsync(data);

            return "Produto cadastrado com sucesso";
        }

        public async Task<List<PedidoCliente>> SelectAll(string login)
        {
            var pedidos = await _collection.Find(p => p.Login == login).ToListAsync();

            return pedidos;
        }

    }
}
