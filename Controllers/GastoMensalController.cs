using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi2026.Entities;
using WebApi2026.Interfaces;

namespace WebApi2026.Controllers
{
    [Route("api/[controller]")]
    public class GastoMensalController : ControllerBase
    {
        // Dependece Injection
        private readonly IGastoMensalService _service;

        public GastoMensalController(IGastoMensalService service)
        {
            _service = service;
        }

        // GET: api/GastoMensal
        [HttpGet]
        public async Task<ActionResult<List<GastoMensal>>> GetAll()
        {
            var gastos = await _service.GetAllAsync();
            return Ok(gastos);
        }

        // GET: api/GastoMensal/{id}
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<GastoMensal>> GetById(string id)
        {
            var gasto = await _service.GetByIdAsync(id);
            if (gasto == null)
                return NotFound();
            return Ok(gasto);
        }

        // POST: api/GastoMensal
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] GastoMensal gastoMensal)
        {
            await _service.CreateAsync(gastoMensal);
            return CreatedAtAction(nameof(GetById), new { id = gastoMensal.Id }, gastoMensal);
        }

        // PUT: api/GastoMensal/{id}
        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> Update(string id, [FromBody] GastoMensal gastoMensal)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            gastoMensal.Id = id; // garante que o ID permaneça o mesmo
            await _service.UpdateAsync(id, gastoMensal);
            return NoContent();
        }

        // DELETE: api/GastoMensal/{id}
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> Delete(string id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
