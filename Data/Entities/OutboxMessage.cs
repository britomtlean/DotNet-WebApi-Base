using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WebApi2026.Entities;

public class OutboxMessage
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Tipo { get; set; } = "";

    public string Destino { get; set; } = "";

    public Pedido Pedido { get; set; } = null!;

    public bool Processado { get; set; } = false;

    public int Tentativas { get; set; } = 0;

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public DateTime? ProcessadoEm { get; set; }
}
