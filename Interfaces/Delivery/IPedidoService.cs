using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApi2026.Entities;

namespace WebApi2026.Interfaces
{
    public interface IPedidoService
    {
        public Task<Boolean> AdicionarPedido(Pedido pedido);

        public Task<Pedido> ConfirmarPedido(string id);

        public Task<Pedido> CancelarPedido(string id);

        public Task<List<Pedido>> RetornarPedido();

        public Task<Pedido> PedidoId(string id);
    }
}
