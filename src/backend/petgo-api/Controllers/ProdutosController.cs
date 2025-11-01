using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using petgo.api.Data;
using petgo.api.Models;
using petgo.api.DTOs; 

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
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            var produtos = await _context.Produtos
                .Include(p => p.CategoriaProduto)
                .ToListAsync();
            
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            var produto = await _context.Produtos
                .Include(p => p.CategoriaProduto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null) return NotFound();
            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> Create(PetCreateDto petDto)
        {
            
            try
            {
            
                if (petDto.Preco < 0)
                {
                
                    return BadRequest("Preço não pode ser negativo.");
                }

                var categoriaExiste = await _context.CategoriasProdutos
                    .AnyAsync(c => c.Id == petDto.CategoriaId);
                
                if (!categoriaExiste)
                {
                    return BadRequest("Categoria não encontrada. Verifique o CategoriaId.");
                }
                var produto = new Produto
                {
                    Nome = petDto.Nome,
                    Descricao = petDto.Descricao,
                    Preco = petDto.Preco,
                    Estoque = petDto.Estoque,
                    Status = StatusProduto.ATIVO, 
                    CategoriaId = petDto.CategoriaId,
                    CategoriaProdutoId = petDto.CategoriaId, 

                    Especie = petDto.Especie,
                    Raca = petDto.Raca,
                    DataNascimento = petDto.DataNascimento,
                    Castrado = petDto.Castrado,
                    Vacinado = petDto.Vacinado,
                    Porte = petDto.Porte,
                    Saude = petDto.Saude,

    
                    Imagens = petDto.Imagens ?? new List<string>(),
                };
                
                
                produto.Id = 0;

                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();

            
                var produtoCriado = await _context.Produtos
                    .Include(p => p.CategoriaProduto)
                    .FirstOrDefaultAsync(p => p.Id == produto.Id);

                return CreatedAtAction(nameof(Get), new { id = produto.Id }, produtoCriado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar produto: {ex.Message}");
                return BadRequest($"Erro interno: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        
        public async Task<ActionResult<Produto>> Update(int id, Produto produto)
        {
            try
            {
                // Verifica se IDs coincidem
                if (id != produto.Id && produto.Id != 0)
                {
                    return BadRequest("ID do produto não confere");
                }

                // Buscar produto existente
                var produtoDb = await _context.Produtos.FindAsync(id);
                if (produtoDb == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado");
                }

                // Validações básicas
                if (string.IsNullOrWhiteSpace(produto.Nome))
                {
                    return BadRequest("Nome é obrigatório");
                }
                
                if (string.IsNullOrWhiteSpace(produto.Descricao))
                {
                    return BadRequest("Descrição é obrigatória");
                }
                
                if (produto.Preco < 0)
                {
                    return BadRequest("Preço não pode ser negativo");
                }
                
                if (produto.Estoque < 0)
                {
                    return BadRequest("Estoque não pode ser negativo");
                }

                // Verificar se categoria existe
                var categoriaExiste = await _context.CategoriasProdutos
                    .AnyAsync(c => c.Id == produto.CategoriaProdutoId);
                
                if (!categoriaExiste)
                {
                    return BadRequest("Categoria não encontrada");
                }

                produtoDb.Nome = produto.Nome;
                produtoDb.Descricao = produto.Descricao;
                produtoDb.Preco = produto.Preco;
                produtoDb.Estoque = produto.Estoque;
                produtoDb.Status = produto.Status;
                produtoDb.CategoriaId = produto.CategoriaProdutoId;
                produtoDb.CategoriaProdutoId = produto.CategoriaProdutoId;
                
                produtoDb.Especie = produto.Especie;
                produtoDb.Raca = produto.Raca;
                produtoDb.DataNascimento = produto.DataNascimento;
                produtoDb.Castrado = produto.Castrado;
                produtoDb.Vacinado = produto.Vacinado;
                produtoDb.Porte = produto.Porte;
                produtoDb.Saude = produto.Saude;
                
                // Atualizar imagens
                produtoDb.Imagens = produto.Imagens ?? new List<string>();

                await _context.SaveChangesAsync();

                // Recarregar produto com categoria
                var produtoAtualizado = await _context.Produtos
                    .Include(p => p.CategoriaProduto)
                    .FirstOrDefaultAsync(p => p.Id == id);

                return Ok(produtoAtualizado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar produto {id}: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return BadRequest($"Erro interno: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var produto = await _context.Produtos.FindAsync(id);
                if (produto == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado");
                }

                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
                
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao deletar produto {id}: {ex.Message}");
                return BadRequest($"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Produto>>> SearchByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Nome de busca é obrigatório");
            }

            var produtos = await _context.Produtos
                .Include(p => p.CategoriaProduto)
                .Where(p => p.Nome.Contains(name) || p.Descricao.Contains(name))
                .ToListAsync();

            return Ok(produtos);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetByCategory(int categoryId)
        {
            var produtos = await _context.Produtos
                .Include(p => p.CategoriaProduto)
                .Where(p => p.CategoriaProdutoId == categoryId)
                .ToListAsync();

            return Ok(produtos);
        }
    }
}