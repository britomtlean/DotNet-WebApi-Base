using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using WebApi2026.Entities;

namespace WebApi2026.Context
{
    public class AppDbContext
    {
        //MYSQL

        // public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // public DbSet<> usuarios { get; set; } = default!;


        //Mongo
        private readonly IMongoDatabase _database;

        public AppDbContext(IConfiguration config)
        {
            var connectionString = config["MongoDbSettings:ConnectionString"];
            var databaseName = config["MongoDbSettings:DatabaseName"];

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        //Tabelas

        public IMongoCollection<Usuario> Usuarios =>
    _database.GetCollection<Usuario>("Usuarios");

    }
}
