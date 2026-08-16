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
        public Task<List<Promocoes>> CriarPromocao(IFormFile file, Promocoes promocao, string login);

        public Task<List<Promocoes>?> ReturnPromocoes(string cpf);
    }
}
