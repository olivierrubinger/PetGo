using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace todoz.api.Models
{
    public enum AlvoTipo
    {
        PASSEADOR,
        SERVICO,
        PRODUTO
    }
    public class Avaliacao
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public AlvoTipo AlvoTipo { get; set; }
        [Required]
        public int AlvoId { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "A nota deve ser entre 1 e 5.")]
        public int Nota { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Comentario { get; set; }
        public string? AutorNome { get; set; }
        [Required]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    }
}