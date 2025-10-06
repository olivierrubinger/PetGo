using Microsoft.EntityFrameworkCore;
using petgo.api.Models;
using System.Text.Json;

namespace petgo.api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<CategoriaProduto> CategoriasProdutos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar Produto
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                
                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(500);
                    
                entity.Property(e => e.Preco)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
                    
                entity.Property(e => e.Estoque)
                    .IsRequired();
                    
                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasConversion<int>();

                entity.Property(e => e.CategoriaId)
                    .IsRequired();
                    
                entity.Property(e => e.CategoriaProdutoId)
                    .IsRequired();

                // Configurar imagens como JSON
                entity.Property(e => e.ImagensJson)
                    .HasColumnName("ImagensJson")
                    .HasDefaultValue("[]");

                // Relacionamento com categoria
                entity.HasOne(e => e.CategoriaProduto)
                    .WithMany()
                    .HasForeignKey(e => e.CategoriaProdutoId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Index para performance
                entity.HasIndex(e => e.Nome);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CategoriaProdutoId);
            });

            // Configurar CategoriaProduto
            modelBuilder.Entity<CategoriaProduto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                
                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                entity.HasIndex(e => e.Nome).IsUnique();
            });
        }
    }
}