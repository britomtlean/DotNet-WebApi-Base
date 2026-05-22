using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi2026.Entities
{
    public class Pedido
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        //public string? FuncionarioId { get; set; }

        //public string? NomeFuncionario { get; set; }

        public DateTime DataPedido { get; set; } = DateTime.UtcNow;

        public List<ProdutoPedido> Produtos { get; set; } = new();

        public double ValorTotal { get; set; }

        public bool? Status { get; set; }

        public string? NomeCliente { get; set; }

        public string? ContatoCliente { get; set; }

        public string? EnderecoCliente { get; set; }

    }
}
