using Microsoft.EntityFrameworkCore;
using todoz.api.Data;
using todoz.api.Models;
using todoz.api.Repositories;

namespace todoz.test
{
    [TestFixture]
    public class TodoInDatabaseTest
    {
        private TodosInDatabase _repository;
        private TodoContext _context;
        private const string DatabaseName = "todoz.test.sqlite3";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Remove o arquivo do banco de dados existir.
            if (File.Exists(DatabaseName))
            {
                File.Delete(DatabaseName);
            }

            // Configura o DbContext para usar o arquivo todoz.test.sqlite3.
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseSqlite($"Data Source={DatabaseName}")
                .Options;

            // Instancia o contexto com as opções configuradas
            _context = new TodoContext(options);
            _repository = new TodosInDatabase(_context);

            // Cria o banco de dados se ele não existir
            _context.Database.EnsureCreated();

        }

        [SetUp]
        public void SetUp()
        {
            // Insere um conjunto de Todos para teste.
            _repository.Add(new Todo { Id = 1, Title = "Aprender C#", Description = "Estudar a estrutura básica do C#", IsComplete = false });
            _repository.Add(new Todo { Id = 2, Title = "Construir uma aplicação", Description = "Utilizar o .net", IsComplete = false });

            _repository.Save();
        }

        [Test]
        public void GetAll_WhenCalled_ReturnsAllItens()
        {
            // Act
            var result = _repository.GetAll();

            // Assert 
            Assert.That(result?.Count, Is.EqualTo(2));
        }

        [Test]
        public void Get_ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var result = _repository.Get(1);

            // Assert 
            Assert.That(result?.Id, Is.EqualTo(1));
            Assert.That(result?.Title, Is.EqualTo("Aprender C#"));
            Assert.That(result?.Description, Is.EqualTo("Estudar a estrutura básica do C#"));
            Assert.IsFalse(result?.IsComplete);

        }

        [Test]
        public void Create_ValidTodoPassed_ReturnsCreatedResponse()
        {
            // Arrange
            Todo todo = new()
            {
                Id = 3,
                Title = "Aprender NUnit",
                Description = "Assistir ao curso em video",
                IsComplete = false
            };

            // Act
            _repository.Add(todo);
            _repository.Save();

            // Assert
            var result = _repository.Get(3);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.That(result.Id, Is.EqualTo(todo.Id));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.That(result.Title, Is.EqualTo(todo.Title));
            Assert.That(result.Description, Is.EqualTo(todo.Description));
            Assert.IsFalse(result.IsComplete); ;
        }

        [Test]
        public void Update_ValidTodoPassed_ReturnsUpdatedTodo()
        {
            // Arrange
            var todo = _repository.Get(1);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            todo.Title = "Aprender NUnit 3.0";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            todo.Description = "Assistir ao curso em video no youtube";
            todo.IsComplete = true;

            // Act
            _repository.Update(todo);
            _repository.Save();

            // Assert
            var result = _repository.Get(1);
            Assert.That(result?.Id, Is.EqualTo(todo.Id));
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.That(result.Title, Is.EqualTo(todo.Title));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.That(result.Description, Is.EqualTo(todo.Description));
            Assert.IsTrue(result.IsComplete); ;
        }

        // Call Setup()
        [Test]
        public void Delete_ExistingIdPassed_ReturnsNullTodo()
        {
            // Arrange
            // Act
            _repository.Delete(1);
            _repository.Save();

            // Assert
            var result = _repository.Get(1);
            Assert.IsNull(result);

        }
        // Call TearDown() 

        [TearDown]
        public void TearDown()
        {
            /// Remove todas as entidades do contexto
            _context.Todos.RemoveRange(_context.Todos);
            _context.SaveChanges();

            // Limpa o ChangeTracker para evitar rastreamento de entidades obsoletas
            foreach (var entry in _context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Fechando manualmente a conexão com o banco de dados
            _context.Database.GetDbConnection().Close();

            // Certificando-se de que o contexto seja descartado corretamente
            _context.Dispose();

        }
    }

}