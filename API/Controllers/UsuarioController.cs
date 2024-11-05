using API_TCC.Data;
using API_TCC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BCrypt.Net;

namespace API_TCC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IJWTAuthenticationManager jwtAuthenticationManager;
        private readonly Contexto _context;

        public UsuarioController(IJWTAuthenticationManager jwtAuthenticationManager, Contexto context)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            _context = context;
        }

        [HttpPost]
        [Route("auth/login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] Usuario usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.Nome) || string.IsNullOrEmpty(usuario.Senha))
            {
                return BadRequest("Nome e senha são obrigatórios.");
            }

            var authResponse = await jwtAuthenticationManager.AuthenticateAsync(usuario.Nome, usuario.Senha);

            if (authResponse == null)
                return Unauthorized("Nome ou senha inválidos");

            return Ok(authResponse);
        }

        [HttpGet]
        [Route("auth/me")]
        [Authorize]
        public async Task<IActionResult> GetMeAsync()
        {
            var nome = User.FindFirstValue(ClaimTypes.Name);
            if (nome == null)
                return Unauthorized();

            var usuario = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Nome == nome);

            if (usuario == null)
                return NotFound("Usuário não encontrado");

            return Ok(new
            {
                usuario.Nome,
                usuario.Permissao
            });
        }

        [HttpGet]
        [Route("usuarios")]
        public async Task<IActionResult> getAllAsync(//consulta geral
           [FromServices] Contexto contexto)
        {
            var e = await contexto
                .Usuarios
                .AsNoTracking()
                .ToListAsync();
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost]
        [Route("usuarios")]
        [Authorize]
        public async Task<IActionResult> PostAsync(
        [FromServices] Contexto contexto,
        [FromBody] Models.Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                contexto.ChangeTracker.AutoDetectChangesEnabled = false;

                // Criptografar a senha antes de salvar
                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

                await contexto.Usuarios.AddAsync(usuario);
                await contexto.SaveChangesAsync();
                contexto.ChangeTracker.AutoDetectChangesEnabled = true; // Reativar a detecção automática de mudanças
                return Created($"api/usuarios/{usuario.Id}", usuario);
            }
            catch (Exception ex)
            {
                contexto.ChangeTracker.AutoDetectChangesEnabled = true; // Reativar a detecção automática de mudanças em caso de erro
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("usuarios/{id}")]
        [Authorize]
        public async Task<IActionResult> PutAsync(
        [FromServices] Contexto contexto,
        [FromBody] Usuario usuario,
        [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model inválida");

            var e = await contexto.Usuarios
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound("Pessoa não encontrada");

            try
            {
                e.Nome = usuario.Nome;

                if (!string.IsNullOrWhiteSpace(usuario.Senha))
                {
                    e.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
                }

                e.Permissao = usuario.Permissao;

                contexto.Usuarios.Update(e);
                await contexto.SaveChangesAsync();
                return Ok(e);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("usuarios/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(//deletar
            [FromServices] Contexto contexto,
            [FromRoute] int id)
        {
            var e = await contexto.Usuarios
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound("Pessoa não encontrada");

            try
            {
                contexto.Usuarios.Remove(e);
                await contexto.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
