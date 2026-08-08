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

        [BsonElement("user")]
        public string User { get; set; } = null!;

        [Required]
        public string Senha { get; set; } = null!;

        [Required]
        public string WhatsApp { get; set; } = null!;

        [Required]
        public string Instagram { get; set; } = null!;

        public string Descricao { get; set; } = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since 1966, when designers at Letraset and James Mosley, the librarian at St Bride Printing Library in London, took a 1914 Cicero translation and scrambled it to make dummy text for Letraset's Body Type sheets.";

        public string Endereco { get; set; } = "Estrada da Matriz, nº 0";

        public string Horario { get; set; } = "Segunda - Sexta: 8:00 ás 17:00 / Sábado: 8:30 ás 12:30";

        public string Logo { get; set; } = "";
    }
}
