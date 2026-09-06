using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using WebApi2026.Entities;

namespace WebApi2026.Interfaces
{
    public interface IPromocoesService
    {
        public Task<Promocoes?> CriarPromocao(IFormFile file, Promocoes promocao);

        public Task<List<Promocoes>?> ReturnPromocoes(string login);
    }
}
