using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace WebApi2026.Entities
{
    public class Gasto
    {
        [BsonElement("nome")]
        public string Nome { get; set; }

        [BsonElement("gasto")]
        public double Valor { get; set; }
    }
}
