using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebApi2026.Entities
{
    public class Cliente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nome")]
        [Required]
        public string Nome { get; set; } = null!;

        [BsonElement("celular")]
        [Required]
        public string Celular { get; set; } = null!;

        [BsonElement("endereco")]
        [Required]
        public string Endereco { get; set; } = null!;

    }
}
