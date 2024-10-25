using API_TCC.Data;
using API_TCC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_TCC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EntregaController : ControllerBase
    {

        private readonly IJWTAuthenticationManager jwtAuthenticationManager;

        public EntregaController(IJWTAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpGet]
        [Route("entregas")]
        public async Task<IActionResult> getAllAsync(//consulta geral
           [FromServices] Contexto contexto)
        {
            var e = await contexto
                .Entregas
                .AsNoTracking()
                .ToListAsync();
            return e == null ? NotFound() : Ok(e);
        }

        [HttpGet]
        [Route("entregas/{nome}")]
        public async Task<IActionResult> getByNomeAsync(//consulta por nome
            [FromServices] Contexto contexto,
            [FromRoute] string nome)
        {
            var entrega = await contexto
                .Entregas
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Nome == nome);

            return entrega == null ? NotFound() : Ok(entrega);
        }

        [HttpGet]
        [Route("entregas/id/{id}")]
        public async Task<IActionResult> getByIdAsync(//consulta por nome
            [FromServices] Contexto contexto,
            [FromRoute] int id)
        {
            var entrega = await contexto
                .Entregas
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            return entrega == null ? NotFound() : Ok(entrega);
        }


        [HttpPost]
        [Route("entregas")]

        public async Task<IActionResult> PostAsync(//cadastro
            [FromServices] Contexto contexto,
            [FromBody] Entrega entrega)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await contexto.Entregas.AddAsync(entrega);
                await contexto.SaveChangesAsync();
                return Created($"api/entregas/{entrega.Id}", entrega);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("entregas/{id}")]
        public async Task<IActionResult> PutAsync(//editar
            [FromServices] Contexto contexto,
            [FromBody] Entrega entrega,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model inválida");
            var e = await contexto.Entregas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound("Pessoa não encontrada");

            try
            {
                e.Nome = entrega.Nome;
                e.Valor = entrega.Valor;
                e.Situacao = entrega.Situacao;

                contexto.Entregas.Update(e);
                await contexto.SaveChangesAsync();
                return Ok(e);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("entregas/{id}")]
        public async Task<IActionResult> DeleteAsync(//deletar
            [FromServices] Contexto contexto,
            [FromRoute] int id)
        {
            var e = await contexto.Entregas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound("Pessoa não encontrada");

            try
            {
                contexto.Entregas.Remove(e);
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
