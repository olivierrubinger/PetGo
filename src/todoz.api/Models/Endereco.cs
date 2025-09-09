namespace todoz.api.Models
{
    public class Endereco
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public string Rua { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public string Pais { get; set; }
    }
}