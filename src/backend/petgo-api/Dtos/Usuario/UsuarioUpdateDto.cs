using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace petgo.api.Dtos.Usuario
{
    public class UsuarioUpdateDto
    {
        [Required(ErrorMessage = "O nome é obrigatório"),
        MaxLength(100)]
        public string Nome { get; set; }  = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório"),
        MaxLength(20)]
        public string Telefone { get; set; }  = string.Empty;

        [MaxLength(500)] // URL da foto
        public string? FotoPerfil { get; set; } 
        [MaxLength(500, ErrorMessage = "A descrição não pode passar de 500 caracteres")]
        public string? Descricao { get; set; } 

        [Range(0, 1000, ErrorMessage = "O valor deve ser entre 0 e 1000")]
        public decimal? ValorCobrado { get; set; }
    }
}