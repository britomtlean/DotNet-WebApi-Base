using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi2026.Interfaces;
using WebApi2026.Entities;

namespace WebApi2026.Controllers
{

    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutosService _service;
        public ProdutosController(IProdutosService service)
        {
            this._service = service;
        }

        // Rotas

        [HttpPost]
        public async Task<List<Produto>> Create([FromForm] Produto produto, IFormFile arquivo)
        {
            var produtos = await _service.AddProduct(produto, arquivo);
            return produtos;
        }
    }
}
