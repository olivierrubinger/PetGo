using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using petgo.api.Dtos.Endereco;
using petgo.api.Dtos.Passeador;
using petgo.api.Dtos.Pet;

namespace petgo.api.Dtos.Usuario
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string? FotoPerfil { get; set; }
        public string Tipo { get; set; } = string.Empty;

        public List<EnderecoDto> Enderecos { get; set; } = [];
        public List<PetDto> Pets { get; set; } = [];
        public PasseadorDto? Passeador { get; set; }
    }
}