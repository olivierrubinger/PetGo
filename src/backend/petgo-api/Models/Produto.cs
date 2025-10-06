using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace todoz.api.Models
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
        public string Nome { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public decimal Preco { get; set; }
        public List<string> Imagens { get; set; } = new List<string>();
        [Required]
        public int CategoriaId { get; set; }
        [Required]
        public int Estoque { get; set; }
        [Required]
        public StatusProduto Status { get; set; }
        public CategoriaProduto CategoriaProduto { get; set; }
    }
}