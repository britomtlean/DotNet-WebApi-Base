using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Nome { get; set; } = null!;
        [Required]
        public string Cpf { get; set; } = null!;
        [Required]
        public string Senha { get; set; } = null!;
    }
}
