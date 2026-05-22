using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using WebApi2026.Entities;

namespace WebApi2026.Context
{
    public class AppDbContext
    {
        private readonly IMongoDatabase _database;

        public AppDbContext(IConfiguration config)
        {
            var connectionString = config["MongoDbSettings:ConnectionString"];
            var databaseName = config["MongoDbSettings:DatabaseName"];

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        ////////////////////////////// TABELAS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public IMongoCollection<Usuario> Usuarios =>
            _database.GetCollection<Usuario>("Usuarios");


        public IMongoCollection<GastoMensal> GastosMensais =>
            _database.GetCollection<GastoMensal>("GastosMensais");

        public IMongoCollection<Produto> Produto =>
            _database.GetCollection<Produto>("Produtos");

        public IMongoCollection<Cliente> Cliente =>
            _database.GetCollection<Cliente>("Clientes");

        public IMongoCollection<Pedido> Pedido =>
            _database.GetCollection<Pedido>("Pedidos");

    }
}
