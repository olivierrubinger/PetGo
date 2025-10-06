using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using todoz.api.Models;

namespace todoz.api.Data
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
    }
}