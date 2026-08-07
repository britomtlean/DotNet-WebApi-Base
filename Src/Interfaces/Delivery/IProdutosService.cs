using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi2026.Entities;

namespace WebApi2026.Interfaces
{
    public interface IProdutosService
    {
        public Task<List<Produto>> AddProduct(Produto produto, IFormFile arquivo, string cpf);

        public Task<List<Produto>?> ReturnProducts(string cpf);

        public Task<Produto?> UpdateProduct(string id, Produto update, string cpf);

        public Task<string> DeleteProduct(string id, string cpf);
    }
}
