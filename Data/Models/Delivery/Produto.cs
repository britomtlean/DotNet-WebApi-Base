using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebApi2026.Entities
{
    public class Produto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nome")]
        [Required]
        public string Nome { get; set; } = null!;

        [BsonElement("descricao")]
        public string? Descricao { get; set; }

        [BsonElement("categoria")]
        public string? Categoria { get; set; }

        [BsonElement("valor")]
        public decimal Valor { get; set; } = 0;

        [BsonElement("estoque")]
        public int Estoque { get; set; } = 0;

        [BsonElement("disponibilidade")]
        public bool Disponibilidade { get; set; } = true;

        [BsonElement("imagem")]
        public string? Imagem { get; set; }

        [BsonElement("data_criacao")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
