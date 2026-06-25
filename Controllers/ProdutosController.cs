using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi2026.Interfaces;
using WebApi2026.Entities;
using Microsoft.AspNetCore.Authorization;

namespace WebApi2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutosService _service;
        public ProdutosController(IProdutosService service)
        {
            this._service = service;
        }

        // Rotas

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Take([FromForm] Produto produto, IFormFile arquivo)
        {
            try
            {
                var produtos = await _service.AddProduct(produto, arquivo);
                return Ok(produtos);
            }
            catch(Exception er)
            {
                return BadRequest(er.Message);
            }
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> Send()
        {
            try{
                var produtos = await _service.ReturnProducts();
                return Ok(produtos);
            }
            catch(Exception er)
            {
                return BadRequest(er.Message);
            }
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] Produto data)
        {
            var message = await _service.UpdateProduct(id, data);
            return Ok(message);
        }
    }
}
