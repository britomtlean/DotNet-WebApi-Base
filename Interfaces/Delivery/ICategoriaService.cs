using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi2026.Entities;

namespace WebApi2026.Interfaces
{
    public interface ICategoriaService
    {
        public Task<string> Create(string login, Categoria data, IFormFile file);

        public Task<List<Categoria>?> SelectAll(string login);
    }
}
