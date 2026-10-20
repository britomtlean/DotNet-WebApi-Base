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
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // REQUISIÇÃO

        public string? Nome { get; set; } = null!;

        [BsonElement("user")]
        public string? User { get; set; } = null!;

        public string? Senha { get; set; } = null!;

        // OPCIONAL

        [MaxLength(16)]
        public string? WhatsApp { get; set; } = "";

        [MaxLength(20)]

        public string? Instagram { get; set; } = "";

        [MaxLength(30)]
        public string? Descricao { get; set; } = "";

        [MaxLength(60)]
        public string? Endereco { get; set; } = "Estrada da Matriz, nº 0";

        [MaxLength(60)]
        public string? Horario { get; set; } = "Segunda - Sexta: 8:00 ás 17:00 / Sábado: 8:30 ás 12:30";

        public string? Logo { get; set; } = "";

        public string? Background { get; set; } = "";

        public string? BackgroundSecond { get; set; } = "";

        public string? Color { get; set; } = "";
    }
}
