using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using petgo.api.Controllers;
using petgo.api.Data;
using petgo.api.Dtos.Usuario;
using petgo.api.Models;

namespace petgo.test
{
    [TestFixture]
    public class UsuariosControllerTests
    {
        private AppDbContext _context = null!;
        private IConfiguration _config = null!;
        private UsuariosController _controller = null!;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString())
                            .Options;

            _context = new AppDbContext(options);
            string senhaHash = BCrypt.Net.BCrypt.HashPassword("senha123");

            _context.Usuarios.Add(new Usuario
            {
                Id = 1,
                Nome = "Usuario Teste",
                Email = "teste@gmail.com",
                Senha = senhaHash,
                Telefone = "123456789",
                Tipo = TipoUsuario.CLIENTE,
                DataCriacao = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var inMemorySettings = new Dictionary<string, string?> {
            {"Jwt:Key", "UMA_CHAVE_FALSA_PARA_TESTES_BEM_LONGA_E_SEGURA_123456789"},
            {"Jwt:Issuer", "petgo.api"},
            {"Jwt:Audience", "petgo.app"}
            };

            _config = new ConfigurationBuilder()
               .AddInMemoryCollection(inMemorySettings)
               .Build();

            _controller = new UsuariosController(_context, _config);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task RegisterUsuario_WhenValidData_ReturnsCreatedUsuario()
        {
            var novoUsuarioDto = new UsuarioCreateDto
            {
                Nome = "Novo Usuário",
                Email = "novo@email.com",
                Senha = "senhaForte123",
                Telefone = "123456789",
                TipoUsuario = (int)TipoUsuario.CLIENTE
            };

            var result = await _controller.RegisterUsuario(novoUsuarioDto);

            var createdResult = result.Result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);

            var usuarioRetornado = createdResult.Value as UsuarioDto;
            Assert.That(usuarioRetornado, Is.Not.Null);
            Assert.That(usuarioRetornado.Email, Is.EqualTo("novo@email.com"));

            // Verifique se o usuário foi salvo no banco de dados falso
            var usuarioDb = await _context.Usuarios
                                            .FirstOrDefaultAsync(u => u.Email == "novo@email.com");
            Assert.That(usuarioDb, Is.Not.Null);
            Assert.That(usuarioDb.Nome, Is.EqualTo("Novo Usuário"));

            // Verifica se a senha foi hasheada e não texto puro
            Assert.That(usuarioDb.Senha, Is.Not.EqualTo("senhaForte123"));
        }

        [Test]
        public async Task RegisterUsuario_WhenEmailAlreadyExists_ReturnsBadRequest()
        {
            var usuarioDtoDuplicado = new UsuarioCreateDto
            {
                Nome = "Usuario Fantasma",
                Email = "teste@gmail.com",
                Senha = "senhaQualquer123",
                Telefone = "000000000",
                TipoUsuario = (int)TipoUsuario.CLIENTE
            };

            var result = await _controller.RegisterUsuario(usuarioDtoDuplicado);

            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null, "O resultado não foi um BadRequestObjectResult");

            var responseValue = badRequestResult.Value;
            Assert.That(responseValue, Is.Not.Null, "O valor do BadRequest estava nulo");

            Assert.That(responseValue, Has.Property("message").EqualTo("Este email já está cadastrado."));

            var count = await _context.Usuarios.CountAsync();
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public async Task Login_WhenCredentialsAreValid_ReturnsTokenAndUsuario()
        {
            var loginDto = new LoginDto
            {
                Email = "teste@gmail.com",
                Senha = "senha123"
            };

            var result = await _controller.Login(loginDto);

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null, "O resultado não foi um OkObjectresult");

            var loginResponse = okResult.Value as LoginResponseDto;
            Assert.That(loginResponse, Is.Not.Null, "A resposta do login (LoginResponseDto) estava nula");

            Assert.Multiple(() =>
            {
                Assert.That(loginResponse.Token, Is.Not.Empty, "O Token JWT estava vazio");

                Assert.That(loginResponse.Usuario.Email, Is.EqualTo("teste@gmail.com"));
            });
        }

        [Test]
        public async Task Login_WhenPasswordIsInvalid_ReturnsUnauthorized()
        {
            var loginDto = new LoginDto
            {
                Email = "teste@email.com",
                Senha = "senha-errada-123"
            };

            var result = await _controller.Login(loginDto);

            var okResult = result.Result as UnauthorizedObjectResult;
            Assert.That(okResult, Is.Not.Null, "O resultado não foi Unauthorized");
        }

        [Test]
        public async Task GetById_WhenIdExists_ReturnsUsuario()
        {
            var result = await _controller.GetById(1);

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null, "Não foi um OkObjectresult");

            var usuarioDto = okResult.Value as UsuarioDto;
            Assert.That(usuarioDto, Is.Not.Null, "O UsuarioDto estava nulo");
            Assert.That(usuarioDto.Id, Is.EqualTo(1));
            Assert.That(usuarioDto.Email, Is.EqualTo("teste@gmail.com"));
        }

        [Test]
        public async Task GetById_WhenIdDoesNotExist_ReturnsNotFound()
        {
            int idQueNaoExiste = 999;

            var result = await _controller.GetById(idQueNaoExiste);

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>(), "Não foi um NotFound");
        }

        [Test]
        public async Task GetUsuarios_ReturnsAllUsuarios()
        {
            var result = await _controller.GetUsuarios();

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null, "Não foi um OkObjectresult");

            var usuariosList = okResult.Value as IEnumerable<UsuarioDto>;
            Assert.That(usuariosList, Is.Not.Null, "A lista de UsuarioDto estava nula");

            Assert.That(usuariosList.Count(), Is.EqualTo(1));

            var primeiroUsuario = usuariosList.First();
            Assert.That(primeiroUsuario.Email, Is.EqualTo("teste@gmail.com"));
        }

        [Test]
        public async Task UpdateUsuario_WhenDataIsValid_ReturnsNoContent()
        {
            var updateDto = new UsuarioUpdateDto
            {
                Nome = "Nome Atualizado",
                Telefone = "111111111",
                FotoPerfil = "http://nova.foto/url"
            };

            var result = await _controller.UpdateUsuario(1, updateDto);
            Assert.That(result, Is.InstanceOf<NoContentResult>(), "A resposta não foi NoContent");

            _context.ChangeTracker.Clear();

            var usuarioAtualizadoDb = await _context.Usuarios.FindAsync(1);
            Assert.That(usuarioAtualizadoDb, Is.Not.Null, "Usuário não encontrado no banco pós update");
            Assert.Multiple(() =>
            {
                Assert.That(usuarioAtualizadoDb.Nome, Is.EqualTo("Nome Atualizado"));
                Assert.That(usuarioAtualizadoDb.Telefone, Is.EqualTo("111111111"));
            });
        }

        [Test]
        public async Task UpdateUsuario_WhenIdDoesNotExist_ReturnsNotFound()
        {
            int idQueNaoExiste = 999;

            var updateDto = new UsuarioUpdateDto
            {
                Nome = "Usuário Fantasma",
                Telefone = "000000000",
                FotoPerfil = "http://foto.fantasma/url"
            };

            var result = await _controller.UpdateUsuario(idQueNaoExiste, updateDto);
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>(), "A resposta não foi NotFoundObjectResult");
        }

        [Test]
        public async Task DeleteUsuario_WhenIdExists_ReturnsNoContent()
        {
            var result = await _controller.DeleteUsuario(1);

            Assert.That(result, Is.InstanceOf<NoContentResult>(), "A resposta não foi NoContent");

            var usuarioDeletado = await _context.Usuarios.FindAsync(1);
            Assert.That(usuarioDeletado, Is.Null, "O usuário não foi deletado do banco");
        }

        [Test]
        public async Task DeleteUsuario_WhenIdDoesNotExist_ReturnsNotFound()
        {
            var idQueNaoExiste = 999;

            var result = await _controller.DeleteUsuario(idQueNaoExiste);
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>(), "A resposta não foi NotFoundObjectResult");
        }
    }
}