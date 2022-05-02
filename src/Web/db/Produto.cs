namespace Web.db
{
    public partial class Produto
    {
        public Produto()
        {
            Venda = new HashSet<Venda>();
        }

        public string Id { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public decimal Precobrl { get; set; }

        public virtual ICollection<Venda> Venda { get; set; }
    }
}
