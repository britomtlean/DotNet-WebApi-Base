using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi2026.Interfaces
{
    public interface IOutboxMessageService
    {
        public Task<List<OutboxMessage>?> Select();
    }
}
