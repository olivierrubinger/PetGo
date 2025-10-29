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

            // Configurar Passeador
            modelBuilder.Entity<Passeador>(entity =>
            {
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

                // Relacionamento com Usuario
                entity.HasOne(passeador => passeador.Usuario)
                    .WithOne(usuario => usuario.Passeador)
                    .HasForeignKey<Passeador>(passeador => passeador.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade); // Se o usuario for apagado o Passeador é apagado junto

                // Relacionamento com ServicoPasseador
                entity.HasMany(passeador => passeador.Servicos)
                    .WithOne(servico => servico.Passeador)
                    .HasForeignKey(servico => servico.PasseadorId)
                    .OnDelete(DeleteBehavior.Cascade); // Se o passeador for apagado seus servicos sao apagados tambem

                // Index para Perfomance
                entity.HasIndex(p => p.AvaliacaoMedia);
                entity.HasIndex(p => p.ValorCobrado);
            });

            // Configurar Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
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
                    .HasMaxLength(256); // Tamanho para um SHA256

                entity.Property(e => e.Telefone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.FotoPerfil)
                    .IsRequired(false)
                    .HasMaxLength(500);

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .IsRequired()
                    .HasDefaultValueSql("datetime('now')");

                // Relacionamento com Endereco
                entity.HasMany(e => e.Enderecos)
                    .WithOne(endereco => endereco.Usuario)
                    .HasForeignKey(endereco => endereco.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade); // Se deleta o usuario deleta seus endereços

                // Relacionamento com Pet
                entity.HasMany(e => e.Pets)
                    .WithOne(pets => pets.Usuario)
                    .HasForeignKey(pets => pets.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict); // Não deixa deletar Usuario se ele tem Pets

                entity.HasOne(e => e.Passeador)
                    .WithOne(passeador => passeador.Usuario)
                    .HasForeignKey<Passeador>(passeador => passeador.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade); // Se deletas Usuario, deleta o Perfil Passeador 

                // Index para perfomance
                entity.HasIndex(e => e.Email) // Impedir q tenha mais de um usuario com o mesmo Email
                    .IsUnique();
                entity.HasIndex(e => e.Nome);
                entity.HasIndex(e => e.Tipo);
            });

            // Configurar Serviço Passeador
            modelBuilder.Entity<ServicoPasseador>(entity =>
            {
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

                // Index para Perfomance
                entity.HasIndex(s => s.TipoServico);
                entity.HasIndex(s => s.Ativo);
            });

            // Configurar Pet
            modelBuilder.Entity<Pet>(entity =>
        {
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
                .IsRequired(false)
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

            // Configurar imagens como JSON
            entity.Property(p => p.FotosJson)
                .IsRequired()
                .HasDefaultValue("[]");

            // Relacionamento com AnuncioDoacao
            entity.HasOne(p => p.AnuncioDoacao)
                .WithOne(a => a.Pet)
                .HasForeignKey<AnuncioDoacao>(a => a.PetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

            // Configurar AnuncioDoacao
            modelBuilder.Entity<AnuncioDoacao>(entity =>
            {
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

                // Index para Performance
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.Moderacao);
            });

            // Configurar Avaliacao
            modelBuilder.Entity<Avaliacao>(entity =>
            {
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
                entity.ToTable(t => t.HasCheckConstraint("CK_Avaliacao_Nota", "[Nota] >= 1 AND [Nota] <= 5"));

                // Strings
                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Comentario)
                    .IsRequired()
                    .HasMaxLength(2500);

                entity.Property(e => e.AutorNome)
                    .IsRequired(false)
                    .HasMaxLength(100);

                entity.Property(e => e.DataCriacao)
                    .IsRequired()
                    .HasDefaultValueSql("datetime('now')");

                // index para Performance
                entity.HasIndex(e => new { e.AlvoTipo, e.AlvoId })
                    .HasDatabaseName("IX_Avaliacao_Alvo");
            });

            // Configurar Endereco
            modelBuilder.Entity<Endereco>(entity =>
            {
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

                // Relacionamento com Usuario
                entity.HasOne(e => e.Usuario)
                    .WithMany(u => u.Enderecos)
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }
}