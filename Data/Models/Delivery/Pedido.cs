using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi2026.Entities
{
    public class Pedido
    {

        ////////////////// GERA AUTOMÁTICO\\\\\\\\\\\\\\\\\\\

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public DateTime DataPedido { get; set; } = DateTime.UtcNow;

        ////////////////////////////////////////////////////

        public List<ProdutoPedido> Produtos { get; set; } = new();

        public double ValorTotal { get; set; }

        /////////////// RECEBE NULL \\\\\\\\\\\\\\\\\\\\\\\
        public bool? Status { get; set; }

        public string? NomeCliente { get; set; }

        public string? ContatoCliente { get; set; }

        public string? EnderecoCliente { get; set; }

        ///////////////////////////////////////////////////

    }
}
