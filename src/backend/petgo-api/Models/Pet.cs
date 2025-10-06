using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace todoz.api.Models
{
    public enum EspeciePet
    {
        CACHORRO,
        GATO,
        OUTRO
    }

    public enum PortePet
    {
        PEQUENO,
        MEDIO,
        GRANDE
    }
    public class Pet
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public EspeciePet Especie { get; set; }
        [Required]
        public string Raca { get; set; }
        [Required]
        public int idadeMeses { get; set; }
        [Required]
        public PortePet Porte { get; set; }
        [Required]
        public string Cidade { get; set; }
        [Required]
        public string Estado { get; set; }
        public List<string> Fotos { get; set; } = new List<string>();
        [Required]
        public string Observacoes { get; set; }
        public Usuario Usuario { get; set; }
        public AnuncioDoacao AnuncioDoacao { get; set; }
    }
}