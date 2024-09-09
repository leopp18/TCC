
using API_TCC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_TCC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntregadorController : ControllerBase
    {

        private readonly IJWTAuthenticationManager jwtAuthenticationManager;

        public EntregadorController(IJWTAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpGet]
        [Route("entregadores")]
        public async Task<IActionResult> getAllAsync(//consulta geral
            [FromServices] Contexto contexto)
        {
            var e = await contexto
                .Entregadores
                .AsNoTracking()
                .ToListAsync();
            return e == null ? NotFound() : Ok(e);
        }

        [HttpGet]
        [Route("entregadores/{nome}")]

        public async Task<IActionResult> getByIdAsync(//consulta por nome
            [FromServices] Contexto contexto,
            [FromRoute] string nome)
        {
            var entregador = await contexto
                .Entregadores
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Nome == nome);

            return entregador == null ? NotFound() : Ok(entregador);
        }


        [HttpPost]
        [Route("entregadores")]

        public async Task<IActionResult> PostAsync(//cadastro
            [FromServices] Contexto contexto,
            [FromBody] Models.Entregadores entregador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await contexto.Entregadores.AddAsync(entregador);
                await contexto.SaveChangesAsync();
                return Created($"api/entregadores/{entregador.Id}", entregador);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("entregadores/{id}")]
        public async Task<IActionResult> PutAsync(//editar
            [FromServices] Contexto contexto,
            [FromBody] Models.Entregadores entregador,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model inválida");
            var e = await contexto.Entregadores
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound("Pessoa não encontrada");

            try
            {
                e.Nome = entregador.Nome;
                e.Sobrenome = entregador.Sobrenome;
                e.Pix = entregador.Pix;
                e.Situacao = entregador.Situacao;

                contexto.Entregadores.Update(e);
                await contexto.SaveChangesAsync();
                return Ok(e);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("entregadores/{id}")]
        public async Task<IActionResult> DeleteAsync(//deletar
            [FromServices] Contexto contexto,
            [FromRoute] int id)
        {
            var e = await contexto.Entregadores
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound("Pessoa não encontrada");

            try
            {
                contexto.Entregadores.Remove(e);
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

