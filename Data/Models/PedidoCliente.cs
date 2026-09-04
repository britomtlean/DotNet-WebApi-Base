using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi2026.Entities
{
public class PedidoCliente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [Required]
        public string Login { get; set; } = null!;

        [Required]
        public string Nome { get; set; } = null!;

        [Required]
        public string Contato { get; set; } = null!;

        [Required]
        public string Produto { get; set; } = null!;

        public DateTime Data { get; set; } = DateTime.UtcNow;

    }
}
