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
                var cpf = User.Identity?.Name; //EXTRAI O CPF CONTIDO NO TOKEN
                if (cpf == null) throw new Exception("Erro de credenciais cadastradas");

                var produtos = await _service.ReturnProducts(cpf);
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
                var cpf = User.Identity?.Name; //EXTRAI O CPF CONTIDO NO TOKEN
                if (cpf == null) throw new Exception("Erro de credenciais cadastradas");

                var produtos = await _service.AddProduct(produto, arquivo, cpf);
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
                var cpf = User.Identity?.Name; //EXTRAI O CPF CONTIDO NO TOKEN
                if (cpf == null) throw new Exception("Erro de credenciais cadastradas");


                var message = await _service.UpdateProduct(id, data, cpf);
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
                var cpf = User.Identity?.Name; //EXTRAI O CPF CONTIDO NO TOKEN
                if (cpf == null) throw new Exception("Erro de credenciais cadastradas");

                var message = await _service.DeleteProduct(id, cpf);
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
