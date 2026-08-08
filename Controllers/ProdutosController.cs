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


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SendAll()
        {
            try
            {
                var user = User.Identity?.Name;
                if (user == null) throw new Exception("Nenhum usuário vinculado a este login");

                var produtos = await _service.ReturnProducts(user);
                return Ok(produtos);
            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
                return BadRequest(er.Message);
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Produto produto, IFormFile arquivo)
        {
            try
            {
                var user = User.Identity?.Name;
                if (user == null) throw new Exception("Nenhum usuário vinculado a este login");

                var produtos = await _service.AddProduct(produto, arquivo, user);
                return Ok(produtos);
            }
            catch(Exception er)
            {
                Console.WriteLine(er.ToString());
                return BadRequest(er.Message);
            }
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromForm] Produto data)
        {
            try
            {
                var user = User.Identity?.Name; //EXTRAI O CPF CONTIDO NO TOKEN
                if (user == null) throw new Exception("Nenhum usuário vinculado a este login");


                var message = await _service.UpdateProduct(id, data, user);
                return Ok(message);
            }
            catch(Exception er)
            {
                Console.WriteLine(er.ToString());
                return BadRequest(er.Message);
            }
        }


        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                var user = User.Identity?.Name;
                if (user == null) throw new Exception("Nenhum usuário vinculado a este login");

                var message = await _service.DeleteProduct(id, user);
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
