using System.ComponentModel.DataAnnotations;

namespace petgo.api.Models
{
    public class CategoriaProduto
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public required string Nome { get; set; }
    }
}