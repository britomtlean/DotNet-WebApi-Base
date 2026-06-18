using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi2026.Entities
{
    public class Usuario
    {
        [BsonId] // Define como ID principal
        [BsonRepresentation(BsonType.ObjectId)] // Converte ObjectId <-> string
        public string? Id { get; set; }

        public string Nome { get; set; } = null!;
        public string Cpf { get; set; } = null!;
        public string Senha { get; set; } = null!;
    }
}
