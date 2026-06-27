using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using WebApi2026.Entities;

namespace WebApi2026.Interfaces
{
    public interface IClienteService
    {
        public Task<Boolean> CadastrarCliente(Cliente cliente);
    }
}
