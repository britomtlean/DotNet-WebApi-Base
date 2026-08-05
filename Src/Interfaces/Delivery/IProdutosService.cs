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

        public Task<List<Produto>> ReturnProducts();

        public Task<Object> UpdateProduct(string id, Produto update, string cpf);

        public Task<bool> DeleteProduct(string id, string cpf);


        //public Task<bool> Up(Produto produto);

       // public Task<bool> Down(Produto produto);
    }
}
