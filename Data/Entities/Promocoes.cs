using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi2026.Entities
{
    public class Promocoes
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Imagem { get; set; }

        // REQUISIÇÃO

        [Required]
        public string Login { get; set; } = null!;

        public string? Descricao { get; set; } = "";

        public bool? Visibilidade { get; set; } = false;

    }
}
