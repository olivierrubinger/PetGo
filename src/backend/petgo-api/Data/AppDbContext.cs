using Microsoft.EntityFrameworkCore;
using petgo.api.Models;

namespace petgo.api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<AnuncioDoacao> AnuncioDoacoes { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<CategoriaProduto> CategoriasProdutos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Passeador> Passeadores { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ServicoPasseador> ServicoPasseadores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================================
            // PRODUTO 
            // ============================================
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.ToTable("Produtos");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(500);
                
                entity.Property(e => e.Especie)
                    .HasMaxLength(50);

                entity.Property(e => e.Raca)
                    .HasMaxLength(100);

                entity.Property(e => e.Porte)
                    .HasMaxLength(50);

                entity.Property(e => e.Saude)
                    .HasMaxLength(500);

                entity.Property(e => e.DataNascimento);
                entity.Property(e => e.Castrado);
                entity.Property(e => e.Vacinado);
                
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

                entity.Property(e => e.ImagensJson)
                    .HasColumnName("ImagensJson")
                    .HasColumnType("jsonb")
                    .HasDefaultValue("[]");

                entity.HasOne(e => e.CategoriaProduto)
                    .WithMany()
                    .HasForeignKey(e => e.CategoriaProdutoId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Índices para performance
                entity.HasIndex(e => e.Nome);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CategoriaProdutoId);
            });

            // ============================================
            // CATEGORIA PRODUTO
            // ============================================
            modelBuilder.Entity<CategoriaProduto>(entity =>
            {
                entity.ToTable("CategoriaProduto");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.Descricao)
                    .HasMaxLength(500);

                entity.HasIndex(e => e.Nome).IsUnique();
            });

            // ============================================
            // PASSEADOR
            // ============================================
            modelBuilder.Entity<Passeador>(entity =>
            {
                entity.ToTable("Passeadores");
                entity.HasKey(e => e.UsuarioId);

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(1500);

                entity.Property(e => e.ValorCobrado)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.AvaliacaoMedia)
                    .IsRequired()
                    .HasDefaultValue(0.0);

                entity.Property(e => e.QuantidadeAvaliacoes)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.HasOne(passeador => passeador.Usuario)
                    .WithOne(usuario => usuario.Passeador)
                    .HasForeignKey<Passeador>(passeador => passeador.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(passeador => passeador.Servicos)
                    .WithOne(servico => servico.Passeador)
                    .HasForeignKey(servico => servico.PasseadorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(p => p.AvaliacaoMedia);
                entity.HasIndex(p => p.ValorCobrado);
            });

            // ============================================
            // USUARIO
            // ============================================
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Senha)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Telefone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.FotoPerfil)
                    .HasMaxLength(500);

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .IsRequired()
                    .HasDefaultValueSql("NOW()"); 

                entity.HasMany(e => e.Enderecos)
                    .WithOne(endereco => endereco.Usuario)
                    .HasForeignKey(endereco => endereco.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Pets)
                    .WithOne(pets => pets.Usuario)
                    .HasForeignKey(pets => pets.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Passeador)
                    .WithOne(passeador => passeador.Usuario)
                    .HasForeignKey<Passeador>(passeador => passeador.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Nome);
                entity.HasIndex(e => e.Tipo);
            });

            // ============================================
            // SERVICO PASSEADOR
            // ============================================
            modelBuilder.Entity<ServicoPasseador>(entity =>
            {
                entity.ToTable("ServicoPasseadores");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(1500);

                entity.Property(e => e.TipoServico)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.Ativo)
                    .IsRequired()
                    .HasDefaultValue(true);

                entity.HasIndex(s => s.TipoServico);
                entity.HasIndex(s => s.Ativo);
            });

            // ============================================
            // PET
            // ============================================
            modelBuilder.Entity<Pet>(entity =>
            {
                entity.ToTable("Pets");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedOnAdd();

                entity.Property(p => p.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(p => p.Raca)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(p => p.Cidade)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(p => p.Estado)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(p => p.Observacoes)
                    .HasMaxLength(1000);

                entity.Property(p => p.Especie)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(p => p.Porte)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(p => p.idadeMeses)
                    .IsRequired();

                // POSTGRESQL: usar jsonb
                entity.Property(p => p.FotosJson)
                    .IsRequired()
                    .HasColumnType("jsonb")
                    .HasDefaultValue("[]");

                entity.HasOne(p => p.AnuncioDoacao)
                    .WithOne(a => a.Pet)
                    .HasForeignKey<AnuncioDoacao>(a => a.PetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ============================================
            // ANUNCIO DOACAO
            // ============================================
            modelBuilder.Entity<AnuncioDoacao>(entity =>
            {
                entity.ToTable("AnuncioDoacoes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.ContatoWhatsapp)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.Moderacao)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.Moderacao);
            });

            // ============================================
            // AVALIACAO
            // ============================================
            modelBuilder.Entity<Avaliacao>(entity =>
            {
                entity.ToTable("Avaliacoes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.AlvoTipo)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.AlvoId)
                    .IsRequired();

                entity.Property(e => e.Nota)
                    .IsRequired();

                // PostgreSQL: Check constraint
                entity.ToTable(t => t.HasCheckConstraint("CK_Avaliacao_Nota", "\"Nota\" >= 1 AND \"Nota\" <= 5"));

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Comentario)
                    .IsRequired()
                    .HasMaxLength(2500);

                entity.Property(e => e.AutorNome)
                    .HasMaxLength(100);

                entity.Property(e => e.DataCriacao)
                    .IsRequired()
                    .HasDefaultValueSql("NOW()");

                entity.HasIndex(e => new { e.AlvoTipo, e.AlvoId })
                    .HasDatabaseName("IX_Avaliacao_Alvo");
            });

            // ============================================
            // ENDERECO
            // ============================================
            modelBuilder.Entity<Endereco>(entity =>
            {
                entity.ToTable("Enderecos");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Rua)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Cep)
                    .IsRequired()
                    .HasMaxLength(9);

                entity.Property(e => e.Pais)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(e => e.Usuario)
                    .WithMany(u => u.Enderecos)
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}