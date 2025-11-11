using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace petgo.api.Dtos.Usuario
{
    public class UsuarioCreateDto
    {
        [Required(ErrorMessage = "O nome é obrigatório"), MaxLength(100)]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [MaxLength(150)]
        public required string Email { get; set; } 

        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        [MaxLength(50)]
        public required string Senha { get; set; } 

        [Required(ErrorMessage = "O telefone é obrigatório"), MaxLength(20)]
        public required string Telefone { get; set; }
        [Required(ErrorMessage = "O tipo de usuário é obrigatório")]
        public int Tipo { get; set; }
        public string? FotoPerfil { get; set; }
    }
}