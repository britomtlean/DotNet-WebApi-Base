using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi2026.Entities;

namespace WebApi2026.Interfaces
{
    public interface IPedidoClienteService
    {
        public Task<string> Insert(string login, PedidoCliente data);

        public Task<List<PedidoCliente>?> SelectAll(string login);
    }
}
