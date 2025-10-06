using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace todoz.api.Models
{
    public class Passeador
    {
        [Key]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public decimal ValorCobrado { get; set; }
        [Required]
        public double AvaliacaoMedia { get; set; }
        [Required]
        public int QuantidadeAvaliacoes { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<ServicoPasseador> Servicos { get; set; } = new List<ServicoPasseador>();
    }
}