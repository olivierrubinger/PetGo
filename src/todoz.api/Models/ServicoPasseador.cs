using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace todoz.api.Models
{
    public class ServicoPasseador
    {
        public enum Tipo
        {
            PASSEIO,
            CRECHE,
            TREINAMENTO
        }

        [Key]
        public int Id { get; set; }
        [ForeignKey("Passeador")]
        public int PasseadorId { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public Tipo TipoServico { get; set; }
        [Required]
        public bool Ativo { get; set; } = true;
        public Passeador Passeador { get; set; }
    }
}