using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace WebApi2026.Entities
{
    public class GastoMensal
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioId { get; set; }

        [BsonElement("mes")]
        public string Mes { get; set; }

        [BsonElement("gastos")]
        public List<Gasto> Gastos { get; set; } = new List<Gasto>();
    }
}
