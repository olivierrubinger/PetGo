using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace todoz.api.Models
{
    public enum TipoUsuario
    {
        CLIENTE,
        PASSEADOR, 
        ADMIN 
    }

    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Senha { get; set; }
        [Required]
        public string Telefone { get; set; }
        [Required]
        public TipoUsuario Tipo { get; set; }
        [Required]
        public string FotoPerfil { get; set; }
        [Required]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public List<Endereco> Enderecos { get; set; } = new List<Endereco>();
        public Passeador? Passeador { get; set; }
        public List<Pet> Pets { get; set; }
    }
}
