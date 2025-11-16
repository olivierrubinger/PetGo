using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using petgo.api.Data;
using petgo.api.Dtos.Endereco;
using petgo.api.Dtos.Passeador;
using petgo.api.Dtos.Pet;
using petgo.api.Dtos.Usuario;
using petgo.api.Models;

namespace petgo.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        // FUNÇÃO AUXILIAR DE MAPEAMENTO 
        private UsuarioDto MapUsuarioToDto(Usuario usuario)
        {
            return new UsuarioDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                FotoPerfil = usuario.FotoPerfil,
                Tipo = (int)usuario.Tipo,

                // Mapeia a sub-lista de Enderecos
                Enderecos = usuario.Enderecos.Select(e => new EnderecoDto
                {
                    Id = e.Id,
                    UsuarioId = e.UsuarioId,
                    Rua = e.Rua,
                    Estado = e.Estado,
                    Cep = e.Cep,
                    Pais = e.Pais
                }).ToList(),

                // Mapeia a sub-lista de Pets
                Pets = usuario.Pets.Select(p => new PetDto
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    Nome = p.Nome,
                    Especie = (int)p.Especie,
                    Raca = p.Raca,
                    idadeMeses = p.idadeMeses,
                    Porte = (int)p.Porte,
                    Cidade = p.Cidade,
                    Estado = p.Estado,
                    Observacoes = p.Observacoes,

                    Fotos = JsonSerializer.Deserialize<List<string>>(p.FotosJson ?? "[]") ?? []
                }).ToList(),

                // Mapeia o sub-objeto Passeador (se existir)
                Passeador = (usuario.Passeador == null) ? null : new PasseadorDto
                {
                    UsuarioId = usuario.Passeador.UsuarioId,
                    Descricao = usuario.Passeador.Descricao,
                    ValorCobrado = usuario.Passeador.ValorCobrado,
                    AvaliacaoMedia = usuario.Passeador.AvaliacaoMedia,
                    QuantidadeAvaliacoes = usuario.Passeador.QuantidadeAvaliacoes,
                    Nome = usuario.Nome,
                    FotoPerfil = usuario.FotoPerfil,
                    Telefone = usuario.Telefone,

                    // Mapeia os serviços do passeador
                    Servicos = usuario.Passeador.Servicos?.Select(s => new ServicoPasseadorDto
                    {
                        Id = s.Id,
                        Titulo = s.Titulo,
                        Descricao = s.Descricao,
                        TipoServico = (int)s.TipoServico
                    }).ToList() ?? [] // Garante que não quebre se Servicos for nulo
                }
            };
        }

        public UsuariosController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                                        .Include(u => u.Enderecos)
                                        .Include(u => u.Pets)
                                        .Include(u => u.Passeador)
                                            .ThenInclude(p => p.Servicos)
                                        .AsNoTracking()
                                        .ToListAsync();

            var usuariosDto = usuarios.Select(usuario => MapUsuarioToDto(usuario));

            return Ok(usuariosDto);
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioDto>> RegisterUsuario(UsuarioCreateDto usuarioDto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == usuarioDto.Email))
            {
                return BadRequest(new { message = "Este email já está cadastrado." });
            }

            if (usuarioDto.TipoUsuario == (int)TipoUsuario.ADMIN)
            {
                return BadRequest(new { message = "Não é permitido registrar-se como Administrador" });
            }

            string senhaHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Senha);

            var usuario = new Usuario
            {
                Nome = usuarioDto.Nome,
                Email = usuarioDto.Email,
                Telefone = usuarioDto.Telefone,
                Senha = senhaHash,
                Tipo = (TipoUsuario)usuarioDto.TipoUsuario,
                FotoPerfil = usuarioDto.FotoPerfil,
                DataCriacao = DateTime.UtcNow
            };

            try
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Retorno sem SENHA
                var usuarioRetorno = new UsuarioDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Telefone = usuario.Telefone,
                    FotoPerfil = usuario.FotoPerfil,
                    Tipo = (int)usuario.Tipo,
                    Enderecos = [],
                    Pets = [],
                    Passeador = null
                };

                return CreatedAtAction(nameof(GetById), new { id = usuarioRetorno.Id }, usuarioRetorno);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar usuário: {ex.Message}");
                return StatusCode(500, new { message = "Ocorreu um erro interno." });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetById(int id)
        {
            var usuario = await _context.Usuarios
                                    .Include(u => u.Enderecos)
                                    .Include(u => u.Pets)
                                    .Include(u => u.Passeador)
                                        .ThenInclude(p => p.Servicos)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }

            var usuarioDto = MapUsuarioToDto(usuario);

            return Ok(usuarioDto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UsuarioDto>> UpdateUsuario(int id, UsuarioUpdateDto usuarioDto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || int.Parse(userIdClaim) != id)
            {
                return Unauthorized(new { message = "Você não tem permissão para editar este perfil." });
            }

            var usuario = await _context.Usuarios
                                            .Include(u => u.Passeador)
                                            .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuários não encontrado. " });
            }

            usuario.Nome = usuarioDto.Nome;
            usuario.Telefone = usuarioDto.Telefone;
            usuario.FotoPerfil = usuarioDto.FotoPerfil;

            if (usuario.Tipo == TipoUsuario.PASSEADOR)
            {
                // 🛑 Se o Passeador não existe, crie a entidade.
                if (usuario.Passeador == null)
                {
                    usuario.Passeador = new Passeador
                    {
                        UsuarioId = usuario.Id,
                        // Inicialize com valores padrão ou DTO (se necessário)
                        Descricao = string.Empty,
                        ValorCobrado = 0.0m,
                        Servicos = new List<ServicoPasseador>()
                    };
                    // Adiciona a nova entidade ao Context para ser rastreada
                    _context.Passeadores.Add(usuario.Passeador);
                }

                // Agora que o Passeador existe (ou foi criado), atualize seus campos.
                if (usuarioDto.Descricao != null)
                {
                    usuario.Passeador.Descricao = usuarioDto.Descricao;
                }
                if (usuarioDto.ValorCobrado != null)
                {
                    usuario.Passeador.ValorCobrado = usuarioDto.ValorCobrado.Value;
                }
            }

            try
            {
                await _context.SaveChangesAsync();

                var usuarioAtualizado = await _context.Usuarios
                    .Include(u => u.Enderecos)
                    .Include(u => u.Pets)
                    .Include(u => u.Passeador)
                        .ThenInclude(p => p.Servicos)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id);

                var usuarioRetornoDto = MapUsuarioToDto(usuarioAtualizado!);

                return Ok(usuarioRetornoDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar usuário: {ex.Message}");
                return StatusCode(500, new { message = "Ocorreu um erro interno." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios
                                    .Include(u => u.Pets)
                                    .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuario não encontrado. " });
            }

            try
            {
                // Primeiro, removemos todos os pets do usuário.
                _context.RemoveRange(usuario.Pets);

                // Agora que esta sem pets, removemos o usuário
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao deletar usuário e seus dados: {ex.Message}");
                return StatusCode(500, new { message = "Ocorreu um erro interno ao deletar o usuário." });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
        {
            var usuario = await _context.Usuarios
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, usuario.Senha))
            {
                return Unauthorized(new { message = "Email ou senha inválidos." });
            }

            var tokenString = GenerateJwtToken(usuario);

            var usuarioDto = new UsuarioDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                FotoPerfil = usuario.FotoPerfil,
                Tipo = (int)usuario.Tipo
            };

            return Ok(new LoginResponseDto
            {
                Token = tokenString,
                Usuario = usuarioDto
            });
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()), // ID do usuário
                    new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                    new Claim(JwtRegisteredClaimNames.Name, usuario.Nome),
                    new Claim(ClaimTypes.Role, usuario.Tipo.ToString()) // A "Role" (CLIENTE, PASSEADOR, ADMIN)
                };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24), // Validade de 24 horas
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}