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

        public Task<Boolean> ConfirmarPedido(Pedido pedido);

        public Task<Boolean> CancelarPedido(Pedido pedido);

        public Task<List<Pedido>> RetornarPedido();

        public Task<Pedido> PedidoId(string id);
    }
}
