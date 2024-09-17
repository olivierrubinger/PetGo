using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

using todoz.api.Controllers;
using todoz.api.Data;
using todoz.api.Models;
using todoz.api.Repositories;

namespace todoz.test
{
    [TestFixture]
    public class TodoInDatabaseTest
    {
        private ITodoRepository _repository;
        private TodoContext _context;
        private string _fileName = "todoz.test.sqlite3";

        [SetUp]
        public void Setup()
        {
            // Se o arquivo do banco de dados já existir, deletá-lo para começar com um banco de dados limpo
            var projectRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\");
            var _databaseFile = Path.Combine(projectRoot, _fileName);

            // Configurando o DbContext para usar o arquivo todoz.test.sqlite3
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseSqlite($"Data Source={_databaseFile}")
                .Options;

            // Instanciando o contexto com as opções configuradas
            _context = new TodoContext(options);
            _repository = new TodoInDatabase(_context);

            // Criando o banco de dados se ele não existir
            _context.Database.EnsureCreated();

            _repository.Add(new Todo { Id = 1, Title = "Aprender C#", Description = "Estudar a estrutura básica do C#", IsComplete = false });
            _repository.Add(new Todo { Id = 2, Title = "Construir uma aplicação", Description = "Utilizar o .net", IsComplete = false });
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            // Limpa o banco de testes
            _repository.Delete(1);
            _repository.Delete(2);

            // Certifique-se de que todas as alterações foram salvas antes de descartar
            _repository.Save();

            // Fechando manualmente a conexão com o banco de dados
            _context.Database.GetDbConnection().Close();

            // Certificando-se de que o contexto seja descartado corretamente
            _context.Dispose();
        }

        [Test]
        public void GetAll_WhenCalled_ReturnsAllItens()
        {
            // Arrange
            // Act
            var result = _repository.GetAll();

            // Assert 
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void Get_ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var result = _repository.Get(1);

            // Assert 
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Title, Is.EqualTo("Aprender C#"));
            Assert.That(result.Description, Is.EqualTo("Estudar a estrutura básica do C#"));
            Assert.IsFalse(result.IsComplete);

        }
        /*
                        [Test]
                        public void Create_ValidObjectPassed_ReturnsCreatedResponse()
                        {
                            // Arrange
                            Todo todo = new()
                            {
                                Title = "Aprender NUnit",
                                Description = "Assistir ao curso de video",
                                IsComplete = false
                            };

                            // Act
                            var result = _controller.Create(todo);

                            // Assert 
                            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
                        }

                        [Test]
                        public void Create_ValidObjectPassed_ReturnsResponseHasCreatedItem()
                        {
                            // Arrange
                            Todo? todo = new()
                            {
                                Title = "Aprender NUnit",
                                Description = "Assistir a videoaula",
                                IsComplete = false
                            };

                            // Act
                            var result = _controller.Create(todo) as CreatedAtActionResult;
                            var actual = result.Value as Todo;

                            // Assert 
                #pragma warning disable NUnit2045 // Use Assert.Multiple
                            Assert.That(actual.Id, Is.EqualTo(3));
                            Assert.That(actual.Title, Is.EqualTo(todo.Title));
                            Assert.That(actual.Description, Is.EqualTo(todo.Description));
                            Assert.That(actual.IsComplete, Is.EqualTo(todo.IsComplete));
                #pragma warning restore NUnit2045 // Use Assert.Multiple
                        }

                        [Test]
                        public void Update_ExistingIdPassed_ReturnsResponsNoContent()
                        {
                            // Arrange
                            Todo? todo = new()
                            {
                                Id = 1,
                                Title = "Pagar conta",
                                Description = "Pagar escola das crianças",
                                IsComplete = false
                            };

                            // Act
                            var result = _controller.Update(1, todo) as NoContentResult;

                            // Assert
                            Assert.That(result, Is.InstanceOf<NoContentResult>());

                        }

                        [Test]
                        public void Delete_ExistingIdPassed_ReturnsResponsNoContent()
                        {
                            // Arrange

                            // Act
                            var result = _controller.Delete(1) as NoContentResult;

                            // Assert
                            Assert.That(result, Is.InstanceOf<NoContentResult>());

                        }
                        */
    }

}