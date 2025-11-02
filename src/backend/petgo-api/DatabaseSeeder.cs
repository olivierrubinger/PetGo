using Microsoft.EntityFrameworkCore;
using petgo.api.Data;
using petgo.api.Models;

namespace petgo.api.Services
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            Console.WriteLine("🌱 Iniciando seed do banco de dados...");

            // Seed Categorias
            if (!await context.CategoriasProdutos.AnyAsync())
            {
                Console.WriteLine("📦 Criando categorias...");
                
                var categorias = new[]
                {
                    new CategoriaProduto 
                    { 
                        Nome = "Ração e Alimentação",
                        Descricao = "Rações, petiscos e suplementos para pets"
                    },
                    new CategoriaProduto 
                    { 
                        Nome = "Brinquedos",
                        Descricao = "Brinquedos interativos e educativos"
                    },
                    new CategoriaProduto 
                    { 
                        Nome = "Acessórios",
                        Descricao = "Camas, cobertores e acessórios diversos"
                    },
                    new CategoriaProduto 
                    { 
                        Nome = "Higiene e Cuidados",
                        Descricao = "Produtos de higiene e cuidados veterinários"
                    },
                    new CategoriaProduto 
                    { 
                        Nome = "Coleiras e Guias",
                        Descricao = "Coleiras, guias e peitorais"
                    }
                };

                await context.CategoriasProdutos.AddRangeAsync(categorias);
                await context.SaveChangesAsync();
                
                Console.WriteLine($"✅ {categorias.Length} categorias criadas!");
            }
            else
            {
                Console.WriteLine("✅ Categorias já existem no banco.");
            }

            // Seed Produtos
            if (!await context.Produtos.AnyAsync())
            {
                Console.WriteLine("📦 Criando produtos de exemplo...");

                var produtos = new[]
                {
                    new Produto
                    {
                        Nome = "Ração Premium Golden para Cães Adultos",
                        Descricao = "Ração super premium completa e balanceada para cães adultos de todas as raças. Ingredientes naturais e alta digestibilidade.",
                        Preco = 89.90m,
                        Estoque = 50,
                        Status = StatusProduto.ATIVO,
                        CategoriaProdutoId = 1, // ← APENAS esta FK
                        ImagensJson = "[]" 
                    },
                    new Produto
                    {
                        Nome = "Brinquedo Kong Classic",
                        Descricao = "Brinquedo resistente e durável, ideal para cães que gostam de mastigar. Material atóxico e seguro.",
                        Preco = 45.50m,
                        Estoque = 25,
                        Status = StatusProduto.ATIVO,
                        CategoriaProdutoId = 2,
                        ImagensJson = "[]"
                    },
                    new Produto
                    {
                        Nome = "Coleira Ajustável Premium",
                        Descricao = "Coleira confortável e resistente com fivela de liberação rápida. Disponível em várias cores.",
                        Preco = 29.90m,
                        Estoque = 30,
                        Status = StatusProduto.ATIVO,
                        CategoriaProdutoId = 5,
                        ImagensJson = "[]"
                    },
                    new Produto
                    {
                        Nome = "Shampoo Pet Care Natural",
                        Descricao = "Shampoo hipoalergênico para pets com pele sensível. Fórmula natural com extratos de camomila.",
                        Preco = 24.90m,
                        Estoque = 40,
                        Status = StatusProduto.ATIVO,
                        CategoriaProdutoId = 4,
                        ImagensJson = "[]"
                    },
                    new Produto
                    {
                        Nome = "Cama Confort Plus para Pets",
                        Descricao = "Cama super macia e confortável, perfeita para o descanso do seu pet. Tecido antialérgico.",
                        Preco = 129.90m,
                        Estoque = 15,
                        Status = StatusProduto.ATIVO,
                        CategoriaProdutoId = 3,
                        ImagensJson = "[]"
                    },
                    new Produto
                    {
                        Nome = "Petiscos Naturais Sabor Frango",
                        Descricao = "Petiscos 100% naturais feitos com peito de frango desidratado. Sem conservantes artificiais.",
                        Preco = 19.90m,
                        Estoque = 60,
                        Status = StatusProduto.ATIVO,
                        CategoriaProdutoId = 1,
                        ImagensJson = "[]"
                    },
                    new Produto
                    {
                        Nome = "Guia Retrátil Automática 5m",
                        Descricao = "Guia retrátil resistente com sistema de trava. Suporta até 30kg. Ideal para passeios.",
                        Preco = 59.90m,
                        Estoque = 20,
                        Status = StatusProduto.ATIVO,
                        CategoriaProdutoId = 5,
                        ImagensJson = "[]"
                    },
                    new Produto
                    {
                        Nome = "Produto em Desenvolvimento",
                        Descricao = "Este produto ainda está sendo finalizado e testado pela nossa equipe. Em breve estará disponível!",
                        Preco = 0.00m,
                        Estoque = 0,
                        Status = StatusProduto.RASCUNHO,
                        CategoriaProdutoId = 1,
                        ImagensJson = "[]"
                    }
                };

                await context.Produtos.AddRangeAsync(produtos);
                await context.SaveChangesAsync();
                
                Console.WriteLine($"✅ {produtos.Length} produtos criados!");
            }
            else
            {
                Console.WriteLine("✅ Produtos já existem no banco.");
            }

            Console.WriteLine("🎉 Seed concluído com sucesso!");
        }
    }
}