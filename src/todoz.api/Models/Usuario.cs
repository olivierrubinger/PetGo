using System.Net;

namespace todoz.api.Models
{
    public enum Role
    {
        Cliente = 1,
        Passeador = 2,
        Admin = 3
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }   
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public Role Tipo { get; set; } 
        public string FotoPerfil { get; set; }
         public DateTime DataCriacao{ get; set; } = DateTime.UtcNow;
        public List<Endereco> Enderecos { get; set; } = new List<Endereco>();
    }
}
