using Microsoft.EntityFrameworkCore;

namespace Web.db
{
    public partial class lojaContext : DbContext
    {
        public lojaContext()
        {
        }

        public lojaContext(DbContextOptions<lojaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Produto> Produto { get; set; } = null!;
        public virtual DbSet<Venda> Venda { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8mb3");

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.ToTable("produto");

                entity.Property(e => e.Id)
                    .HasMaxLength(12)
                    .HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .HasColumnName("descricao");

                entity.Property(e => e.Precobrl)
                    .HasPrecision(10)
                    .HasColumnName("precobrl");
            });

            modelBuilder.Entity<Venda>(entity =>
            {
                entity.ToTable("venda");

                entity.HasIndex(e => e.ProdutoId, "fk_venda_produto_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cotacaousd)
                    .HasPrecision(10)
                    .HasColumnName("cotacaousd");

                entity.Property(e => e.Precobrl)
                    .HasPrecision(10)
                    .HasColumnName("precobrl");

                entity.Property(e => e.ProdutoId)
                    .HasMaxLength(12)
                    .HasColumnName("produto_id");

                entity.Property(e => e.Quantidade).HasColumnName("quantidade");

                entity.Property(e => e.Taxabrl)
                    .HasPrecision(10)
                    .HasColumnName("taxabrl");

                entity.Property(e => e.Totalbrl)
                    .HasPrecision(10)
                    .HasColumnName("totalbrl");

                entity.HasOne(d => d.Produto)
                    .WithMany(p => p.Venda)
                    .HasForeignKey(d => d.ProdutoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_venda_produto");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
