using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todoz.api.Models
{
    public class Endereco
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        [Required]
        public string Rua { get; set; }
        [Required]
        public string Estado { get; set; }
        [Required]
        public string Cep { get; set; }
        [Required]
        public string Pais { get; set; }

        public Usuario Usuario { get; set; }
    }
}