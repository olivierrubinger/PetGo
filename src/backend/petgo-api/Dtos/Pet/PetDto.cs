using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace petgo.api.Dtos.Pet
{
    public class PetDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        
        public string Especie { get; set; } = string.Empty;
        public string Raca { get; set; } = string.Empty;
        public int idadeMeses { get; set; }
        public string Porte { get; set; } = string.Empty;
        
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
        
        public List<string> Fotos { get; set; } = [];
    }
}