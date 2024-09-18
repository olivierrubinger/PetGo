using Microsoft.EntityFrameworkCore;
using todoz.api.Models;

namespace todoz.api.Data
{
    // Classe base que define o contexto para os banco de dados.
    public class TodoContext : DbContext
    {
        // Construtor que passa as opções para a classe base DbContext
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        // DbSet que mapeia a entidade Todo para o banco de dados
        public DbSet<Todo> Todos { get; set; }
    }
}