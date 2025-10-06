using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace todoz.api.Models
{
    public enum Status
    {
        ATIVO,
        PAUSADO,
        ADOTADO,
        REMOVIDO
    }

    public enum Moderacao
    {
        PENDENTE,
        APROVADO,
        REJEITADO
    }
    public class AnuncioDoacao
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Pet")]
        public int PetId { get; set; }
        [Required]
        public Status Status { get; set; }
        [Required]
        public string Descricao { get; set; }
        public string? ContatoWhatsapp { get; set; }
        [Required]
        public Moderacao Moderacao { get; set; }
        public Pet Pet { get; set; }
    }
}