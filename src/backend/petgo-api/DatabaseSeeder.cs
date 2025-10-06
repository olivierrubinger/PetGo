using Microsoft.EntityFrameworkCore;
using petgo.api.Data;
using petgo.api.Models;

namespace petgo.api.Services
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Criar categorias se não existirem
            if (!await context.CategoriasProdutos.AnyAsync())
            {
                var categorias = new[]
                {
                    new CategoriaProduto { Nome = "Ração e Alimentação" },
                    new CategoriaProduto { Nome = "Brinquedos" },
                    new CategoriaProduto { Nome = "Acessórios" },
                    new CategoriaProduto { Nome = "Higiene e Cuidados" },
                    new CategoriaProduto { Nome = "Coleiras e Guias" }
                };

                await context.CategoriasProdutos.AddRangeAsync(categorias);
                await context.SaveChangesAsync();
            }

            // Criar produtos de exemplo se não existirem (SEM IMAGENS inicialmente)
            if (!await context.Produtos.AnyAsync())
            {
                var produtos = new[]
                {
                    new Produto
                    {
                        Nome = "Ração Premium Golden para Cães Adultos",
                        Descricao = "Ração super premium completa e balanceada para cães adultos de todas as raças. Ingredientes naturais e alta digestibilidade.",
                        Preco = 89.90m,
                        Estoque = 50,
                        Status = StatusProduto.ATIVO,
                        CategoriaId = 1,
                        CategoriaProdutoId = 1,
                        Imagens = new List<string>() 
                    },
                    new Produto
                    {
                        Nome = "Brinquedo Kong Classic",
                        Descricao = "Brinquedo resistente e durável, ideal para cães que gostam de mastigar. Material atóxico e seguro.",
                        Preco = 45.50m,
                        Estoque = 25,
                        Status = StatusProduto.ATIVO,
                        CategoriaId = 2,
                        CategoriaProdutoId = 2,
                        Imagens = new List<string>()
                    },
                    new Produto
                    {
                        Nome = "Coleira Ajustável Premium",
                        Descricao = "Coleira confortável e resistente com fivela de liberação rápida. Disponível em várias cores.",
                        Preco = 29.90m,
                        Estoque = 30,
                        Status = StatusProduto.ATIVO,
                        CategoriaId = 5,
                        CategoriaProdutoId = 5,
                        Imagens = new List<string>()
                    },
                    new Produto
                    {
                        Nome = "Shampoo Pet Care Natural",
                        Descricao = "Shampoo hipoalergênico para pets com pele sensível. Fórmula natural com extratos de camomila.",
                        Preco = 24.90m,
                        Estoque = 40,
                        Status = StatusProduto.ATIVO,
                        CategoriaId = 4,
                        CategoriaProdutoId = 4,
                        Imagens = new List<string>()
                    },
                    new Produto
                    {
                        Nome = "Produto em Desenvolvimento",
                        Descricao = "Este produto ainda está sendo finalizado e testado pela nossa equipe.",
                        Preco = 19.90m,
                        Estoque = 0,
                        Status = StatusProduto.RASCUNHO,
                        CategoriaId = 1,
                        CategoriaProdutoId = 1,
                        Imagens = new List<string>()
                    }
                };

                await context.Produtos.AddRangeAsync(produtos);
                await context.SaveChangesAsync();
            }
        }
    }
}