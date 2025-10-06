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

            // Criar produtos de exemplo se não existirem
            if (!await context.Produtos.AnyAsync())
            {
                var produtos = new[]
                {
                    new Produto
                    {
                        Nome = "Ração Premium Golden para Cães Adultos",
                        Descricao = "Ração super premium completa e balanceada para cães adultos de todas as raças.",
                        Preco = 89.90m,
                        Estoque = 50,
                        Status = StatusProduto.ATIVO,
                        CategoriaId = 1,
                        CategoriaProdutoId = 1,
                        Imagens = new List<string> { 
                            "https://picsum.photos/300/300?random=1&blur=0",
                            "https://picsum.photos/300/300?random=2&blur=0" 
                        }
                    },
                    new Produto
                    {
                        Nome = "Brinquedo Kong Classic",
                        Descricao = "Brinquedo resistente e durável, ideal para cães que gostam de mastigar.",
                        Preco = 45.50m,
                        Estoque = 25,
                        Status = StatusProduto.ATIVO,
                        CategoriaId = 2,
                        CategoriaProdutoId = 2,
                        Imagens = new List<string> { 
                            "https://picsum.photos/300/300?random=3&blur=0" 
                        }
                    },
                    new Produto
                    {
                        Nome = "Coleira Ajustável Premium",
                        Descricao = "Coleira confortável e resistente com fivela de liberação rápida.",
                        Preco = 29.90m,
                        Estoque = 30,
                        Status = StatusProduto.ATIVO,
                        CategoriaId = 5,
                        CategoriaProdutoId = 5,
                        Imagens = new List<string> { 
                            "https://picsum.photos/300/300?random=4&blur=0" 
                        }
                    },
                    new Produto
                    {
                        Nome = "Produto em Rascunho",
                        Descricao = "Este produto ainda está sendo finalizado.",
                        Preco = 19.90m,
                        Estoque = 10,
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