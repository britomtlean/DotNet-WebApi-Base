using System;
using System.Collections.Generic;
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

        [BsonElement("login")]
        public string Login { get; set; } = null!;

        [BsonElement("descricao")]
        public string? Descricao { get; set; } = "";

        [BsonElement("enderecoDaImagem")]
        public string? EnderecoDaImagem { get; set; } = "";

        [BsonElement("visibilidade")]
        public bool? Visibilidade { get; set; } = false;

    }
}
