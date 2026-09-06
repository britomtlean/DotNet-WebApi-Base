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

        [BsonElement("data_criacao")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        [Required]
        [BsonElement("login")]
        public string? Login { get; set; }

        [BsonElement("imagem")]
        public string? Imagem { get; set; }

        // REQUISIÇÃO

        [Required]
        [BsonElement("nome")]
        public string Nome { get; set; } = null!;

        [Required]
        [BsonElement("valor")]
        public string Valor { get; set; } = null!;

        // OPCIONAL

        [BsonElement("codigoBarra")]
        public List<string>? CodigoBarra { get; set; } = new();

        [BsonElement("descricao")]
        public string? Descricao { get; set; } = "";

        [BsonElement("categoria")]
        public string? Categoria { get; set; } = "";

        [BsonElement("estoque")]
        public int? Estoque { get; set; } = 0;

        [BsonElement("disponibilidade")]
        public bool? Disponibilidade { get; set; } = false;

        public bool? Destaque { get; set; } = false;
    }
}
