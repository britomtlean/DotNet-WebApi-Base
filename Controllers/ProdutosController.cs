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


        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> TakeAndCreate([FromForm] Produto produto, IFormFile arquivo)
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
        public async Task<IActionResult> SendAll()
        {
            try
            {
                var produtos = await _service.ReturnProducts();
                return Ok(produtos);
            }
            catch(Exception er)
            {
                return BadRequest(er.Message);
            }
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> TakeAndUpdate([FromRoute] string id, [FromForm] Produto data)
        {
            try
            {
                var message = await _service.UpdateProduct(id, data);
                return Ok(message);
            }
            catch(Exception er)
            {
                Console.WriteLine(er.ToString());
                return BadRequest(er.Message);
            }
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                var message = await _service.DeleteProduct(id);
                return Ok(message);
            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
                return BadRequest(er.Message);
            }
        }
    }
}
