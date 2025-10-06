using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using petgo.api.Models;

namespace petgo.api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Passeador> Passeadores { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<ServicoPasseador> ServicosPasseadores { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<AnuncioDoacao> AnunciosDoacoes { get; set; }
        public DbSet<CategoriaProduto> CategoriasProdutos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Produto>()
                .Property(p => p.Preco)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Passeador>()
                .Property(p => p.ValorCobrado)
                .HasPrecision(18, 2);

            var stringListComparer = new ValueComparer<List<string>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            modelBuilder.Entity<Pet>()
                .Property(p => p.Fotos)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
                )
                .Metadata.SetValueComparer(stringListComparer);

            modelBuilder.Entity<Produto>()
                .Property(p => p.Imagens)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
                )
                .Metadata.SetValueComparer(stringListComparer);
        }
    }
}