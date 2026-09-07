using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using WebApi2026.Context;
using WebApi2026.Interfaces;

namespace WebApi2026.Services
{
    public class OutboxMessageService: IOutboxMessageService
    {

        private readonly IMongoCollection<OutboxMessage> _collection;

        public OutboxMessageService(AppDbContext context)
        {
            _collection = context.outboxMessage;
        }

        public async Task<List<OutboxMessage>?> Select()
        {
            var mensagens = await _collection.Find(x => !x.Processado).ToListAsync();

            return mensagens;

        }

    }
}
