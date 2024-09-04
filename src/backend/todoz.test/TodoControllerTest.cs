using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

using todoz.api.Controllers;
using todoz.api.Models;
using todoz.api.Repositories;


namespace todoz.test
{
    [TestFixture]
    public class TodoControllerTest
    {
        private ITodoRepository _repository;
        private TodoController _controller;

        [SetUp]
        public void Setup()
        {
            _repository = new TodoInMemory();
            _controller = new TodoController(_repository);
        }

        [Test]
        public void GetAll_WhenCalled_ReturnsAllItens()
        {
            // Act
#pragma warning disable CS8602
            var result = _controller.GetAll();
            IEnumerable<Todo>? actual = (result.Result as OkObjectResult).Value as IEnumerable<Todo>;

            // Assert 
            Assert.That(actual.Count, Is.EqualTo(2));
#pragma warning restore CS8602
        }

        [Test]
        public void Get_ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var result = _controller.Get(1);
            Todo? actual = (result.Result as OkObjectResult).Value as Todo;

            // Assert 
            Assert.That(actual.Id, Is.EqualTo(1));
            Assert.That(actual.Title, Is.EqualTo("Aprender C#"));
        }

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
    }
}