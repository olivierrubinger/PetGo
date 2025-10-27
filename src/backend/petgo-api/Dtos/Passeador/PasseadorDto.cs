using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace petgo.api.Dtos.Passeador
{
    public class PasseadorDto
    {
        // Dados do Passeador
        public int UsuarioId { get; set; }
        public  required string Descricao { get; set; }
        public decimal ValorCobrado { get; set; }
        public double AvaliacaoMedia { get; set; }
        public int QuantidadeAvaliacoes { get; set; }

        // Dados do Usuário 
        public required string Nome { get; set; }
        public required string FotoPerfil { get; set; }
        public required string Telefone { get; set; }


        public List<ServicoPasseadorDto> Servicos { get; set; } = new List<ServicoPasseadorDto>();
    }
}