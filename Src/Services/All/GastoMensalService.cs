using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using WebApi2026.Context;
using WebApi2026.Entities;
using WebApi2026.Interfaces;

namespace WebApi2026.Services
{
    public class GastoMensalService : IGastoMensalService
    {
        private readonly IMongoCollection<GastoMensal> _gastosMensais;

        public GastoMensalService(AppDbContext context)
        {
            _gastosMensais = context.GastosMensais;
        }



        public async Task<List<GastoMensal>> GetAllAsync()
        {
            return await _gastosMensais.Find(_ => true).ToListAsync();
        }

        public async Task<GastoMensal> GetByIdAsync(string id)
        {
            return await _gastosMensais.Find(g => g.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(GastoMensal gastoMensal)
        {
            await _gastosMensais.InsertOneAsync(gastoMensal);
        }

        public async Task UpdateAsync(string id, GastoMensal gastoMensal)
        {
            await _gastosMensais.ReplaceOneAsync(g => g.Id == id, gastoMensal);
        }

        public async Task DeleteAsync(string id)
        {
            await _gastosMensais.DeleteOneAsync(g => g.Id == id);
        }
    }
}
