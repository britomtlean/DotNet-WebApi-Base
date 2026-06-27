using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using WebApi2026.Entities;
using WebApi2026.Interfaces;
using MongoDB.Driver; //Mongo
using WebApi2026.Context; //Context

namespace WebApi2026.Services
{
    public class ClienteService : IClienteService
    {

        private readonly IMongoCollection<Cliente> _cliente; //Conexão com a tabela

        public ClienteService(AppDbContext context) //Construtor
        {
            _cliente = context.Cliente;
        }


        public async Task<Boolean> CadastrarCliente(Cliente cliente)
        {
            await this._cliente.InsertOneAsync(cliente);
            return true;
        }
    }
}
