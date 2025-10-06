using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace petgo.api.Models
{
    public enum StatusProduto
    {
        ATIVO,
        RASCUNHO
    }
    
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public required string Nome { get; set; }
        
        [Required]
        public required string Descricao { get; set; }
        
        [Required]
        public decimal Preco { get; set; }
        
        public List<string> Imagens { get; set; } = new List<string>();
        
        [Required]
        public int CategoriaId { get; set; }
        
        [Required]
        public int Estoque { get; set; }
        
        [Required]
        public StatusProduto Status { get; set; }
        
        [Required]
        public int CategoriaProdutoId { get; set; }
        
        public CategoriaProduto? CategoriaProduto { get; set; }
    }
}