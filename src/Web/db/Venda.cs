namespace Web.db
{
    public partial class Venda
    {
        public int Id { get; set; }
        public string ProdutoId { get; set; } = null!;
        public int Quantidade { get; set; }
        public decimal Precobrl { get; set; }
        public decimal Cotacaousd { get; set; }
        public decimal Taxabrl { get; set; }
        public decimal Totalbrl { get; set; }

        public virtual Produto Produto { get; set; } = null!;
    }
}
