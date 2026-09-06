using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi2026.Interfaces;
using WebApi2026.Entities;

namespace WebApi2026.Controllers
{
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _service;

        public CategoriaController(ICategoriaService service)
        {
            _service = service;
        }


        //*********************** ROTAS ***********************//

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Categoria data, IFormFile file)
        {
            try
            {
                var login = User.Identity?.Name;
                if (login == null) throw new Exception("Nenhum usuário vinculado a este login");

                var response = await _service.Create(login, data, file);

                return Ok(response);
            }
            catch(Exception er)
            {
                Console.WriteLine(er.ToString());
                return BadRequest(er.Message);
            }
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var login = User.Identity?.Name;
                if (login == null) throw new Exception("Nenhum usuário vinculado a este login");

                var response = await _service.SelectAll(login);
                return Ok(response);
            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
                return BadRequest(er.Message);
            }
        }

    }
}
