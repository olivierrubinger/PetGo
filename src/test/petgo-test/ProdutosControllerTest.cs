using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todoz.api.Controllers;
using todoz.api.Data;
using todoz.api.Models;

namespace todoz.test
{
    [TestFixture]
    public class ProdutosControllerTest
    {
        private AppDbContext _context;
        private ProdutosController _controller;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            _context.Produtos.Add(new Produto
            {
                Id = 1,
                Nome = "Ração",
                Descricao = "Ração para cães",
                Preco = 50,
                Estoque = 10,
                CategoriaId = 1,
                Status = StatusProduto.ATIVO
            });

            await _context.SaveChangesAsync();

            _controller = new ProdutosController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetProdutos_ReturnsAllItens()
        {
            var result = await _controller.GetProdutos();
            var okResult = result as OkObjectResult;
            var produtos = okResult?.Value as List<Produto>;

            Assert.That(produtos, Is.Not.Null);
            Assert.That(produtos.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetById_ExistingId_ReturnsProduto()
        {
            var result = await _controller.GetById(1);
            var okResult = result.Result as OkObjectResult;
            var produto = okResult?.Value as Produto;

            Assert.That(produto, Is.Not.Null);

            Assert.That(produto.Id, Is.EqualTo(1));
            Assert.That(produto.Nome, Is.EqualTo("Ração"));
        }

        [Test]
        public async Task Create_WhenValidProdutoPassed_ReturnsCreatedProduto()
        {
            var novoProduto = new Produto
            {
                Id = 2,
                Nome = "Coleira",
                Descricao = "Coleira para cães",
                Preco = 30,
                Estoque = 15,
                CategoriaId = 1,
                Status = StatusProduto.ATIVO
            };

            var result = await _controller.Create(novoProduto) as CreatedAtActionResult;
            var produtoCriado = result?.Value as Produto;

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(produtoCriado, Is.Not.Null);
                Assert.That(produtoCriado.Id, Is.EqualTo(novoProduto.Id));
                Assert.That(produtoCriado.Nome, Is.EqualTo(novoProduto.Nome));
                Assert.That(produtoCriado.Preco, Is.EqualTo(novoProduto.Preco));
                Assert.That(produtoCriado.Estoque, Is.EqualTo(novoProduto.Estoque));
                Assert.That(produtoCriado.CategoriaId, Is.EqualTo(novoProduto.CategoriaId));
                Assert.That(produtoCriado.Status, Is.EqualTo(novoProduto.Status));
            });

            var produtoDb = await _context.Produtos.FindAsync(novoProduto.Id);
            Assert.That(produtoDb, Is.Not.Null);
            Assert.That(produtoDb.Nome, Is.EqualTo("Coleira"));
        }

        [Test]
        public async Task Update_ExistingProduto_ReturnsNoContent()
        {
            var produtoAtualizado = new Produto
            {
                Id = 1,
                Nome = "Ração Premium",
                Descricao = "Ração de alta qualidade",
                Preco = 70,
                Estoque = 20,
                CategoriaId = 1,
                Status = StatusProduto.ATIVO
            };

            var result = await _controller.Update(1, produtoAtualizado) as NoContentResult;

            var produtoDb = await _context.Produtos.FindAsync(1);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<NoContentResult>());
                Assert.That(produtoDb.Nome, Is.EqualTo("Ração Premium"));
                Assert.That(produtoDb.Descricao, Is.EqualTo("Ração de alta qualidade"));
                Assert.That(produtoDb.Preco, Is.EqualTo(70));
                Assert.That(produtoDb.Estoque, Is.EqualTo(20));
                Assert.That(produtoDb.CategoriaId, Is.EqualTo(1));
                Assert.That(produtoDb.Status, Is.EqualTo(StatusProduto.ATIVO));  
            });

        }

        [Test]
        public async Task Delete_ExistingProduto_ReturnsNoContent()
        {
            var result = await _controller.Delete(1) as NoContentResult;

            var produtoDb = await _context.Produtos.FindAsync(1);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
            Assert.That(produtoDb, Is.Null);
        }
    }
}