using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace petgo.api.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; }

        [Required]
        public int Estoque { get; set; }

        [Required]
        public StatusProduto Status { get; set; }

        // Campos de categoria - ambos para compatibilidade
        [Required]
        public int CategoriaId { get; set; }
        
        [Required]
        public int CategoriaProdutoId { get; set; }

        // Relacionamento
        public CategoriaProduto? CategoriaProduto { get; set; }

        // Imagens como JSON
        private string _imagensJson = "[]";

        [Column("ImagensJson")]
        public string ImagensJson
        {
            get => _imagensJson;
            set
            {
                _imagensJson = value ?? "[]";
                try
                {
                    Imagens = JsonSerializer.Deserialize<List<string>>(value ?? "[]") ?? new List<string>();
                }
                catch
                {
                    Imagens = new List<string>();
                }
            }
        }

        [NotMapped]
        public List<string> Imagens
        {
            get
            {
                try
                {
                    return JsonSerializer.Deserialize<List<string>>(_imagensJson) ?? new List<string>();
                }
                catch
                {
                    return new List<string>();
                }
            }
            set
            {
                try
                {
                    _imagensJson = JsonSerializer.Serialize(value ?? new List<string>());
                }
                catch
                {
                    _imagensJson = "[]";
                }
            }
        }
    }

    public enum StatusProduto
    {
        ATIVO = 0,
        RASCUNHO = 1
    }
}