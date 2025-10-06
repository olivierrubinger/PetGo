using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using petgo.api.Data;
using petgo.api.Models;

namespace petgo.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetProdutos()
        {
            var produtos = await _context.Produtos.ToListAsync();
            return Ok(produtos);
        }


        [HttpPost]
        public async Task<ActionResult> Create(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetById", new { id = produto.Id }, produto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetById(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            return produto == null ? NotFound() : Ok(produto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Produto produto)
        {
            if (id != produto.Id) return BadRequest();
            var produtoDb = await _context.Produtos.FindAsync(id);
            if (produtoDb == null) return NotFound();

            produtoDb.Nome = produto.Nome;
            produtoDb.Descricao = produto.Descricao;
            produtoDb.Preco = produto.Preco;
            produtoDb.Estoque = produto.Estoque;
            produtoDb.Status = produto.Status;
            produtoDb.CategoriaId = produto.CategoriaId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null) return NotFound();

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}