using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi2026.Entities;

namespace WebApi2026.Interfaces
{
    public interface IGastoMensalService
    {
        Task<List<GastoMensal>> GetAllAsync();
        Task<GastoMensal> GetByIdAsync(string id);
        Task CreateAsync(GastoMensal gastoMensal);
        Task UpdateAsync(string id, GastoMensal gastoMensal);
        Task DeleteAsync(string id);
    }
}

