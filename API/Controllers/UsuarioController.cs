using API_TCC.Data;
using API_TCC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_TCC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IJWTAuthenticationManager jwtAuthenticationManager;

        public UsuarioController(IJWTAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
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

        [HttpGet]
        [Route("usuarios/{nome}")]

        public async Task<IActionResult> getByIdAsync(//consulta por nome
            [FromServices] Contexto contexto,
            [FromRoute] string nome)
        {
            var usuario = await contexto
                .Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Nome == nome);

            return usuario == null ? NotFound() : Ok(usuario);
        }


        [HttpPost]
        [Route("usuarios")]

        public async Task<IActionResult> PostAsync(//cadastro
            [FromServices] Contexto contexto,
            [FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await contexto.Usuarios.AddAsync(usuario);
                await contexto.SaveChangesAsync();
                return Created($"api/usuarios/{usuario.Id}", usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("usuarios/{id}")]
        public async Task<IActionResult> PutAsync(//editar
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
                e.Senha = usuario.Senha;
                e.Permissao = usuario.Permissao; //criptografar aqui?

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
