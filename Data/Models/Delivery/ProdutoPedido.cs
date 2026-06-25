namespace  WebApi2026.Entities
{
    public class ProdutoPedido
    {
        public string ProdutoId { get; set; }

        public string Nome { get; set; }

        public int Quantidade { get; set; }

        public double ValorUnitario { get; set; }

        public double Subtotal { get; set; }
    }
}
