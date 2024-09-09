
using API_TCC.Data;
using API_TCC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_TCC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CidadeController : ControllerBase
    {

        private readonly IJWTAuthenticationManager jwtAuthenticationManager;

        public CidadeController(IJWTAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpGet]
        [Route("cidades")]
        public async Task<IActionResult> getAllAsync(//consulta geral
           [FromServices] Contexto contexto)
        {
            var e = await contexto
                .Cidades
                .AsNoTracking()
                .ToListAsync();
            return e == null ? NotFound() : Ok(e);
        }

        [HttpGet]
        [Route("cidades/{nome}")]

        public async Task<IActionResult> getByIdAsync(//consulta por nome
            [FromServices] Contexto contexto,
            [FromRoute] string nome)
        {
            var cidade = await contexto
                .Cidades
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Nome == nome);

            return cidade == null ? NotFound() : Ok(cidade);
        }


        [HttpPost]
        [Route("cidades")]

        public async Task<IActionResult> PostAsync(//cadastro
            [FromServices] Contexto contexto,
            [FromBody] Cidade cidade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await contexto.Cidades.AddAsync(cidade);
                await contexto.SaveChangesAsync();
                return Created($"api/cidades/{cidade.Id}", cidade);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("cidades/{id}")]
        public async Task<IActionResult> PutAsync(//editar
            [FromServices] Contexto contexto,
            [FromBody] Cidade cidade,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model inválida");
            var e = await contexto.Cidades
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound("Pessoa não encontrada");

            try
            {
                e.Nome = cidade.Nome;
                e.Situacao = cidade.Situacao;

                contexto.Cidades.Update(e);
                await contexto.SaveChangesAsync();
                return Ok(e);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("cidades/{id}")]
        public async Task<IActionResult> DeleteAsync(//deletar
            [FromServices] Contexto contexto,
            [FromRoute] int id)
        {
            var e = await contexto.Cidades
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound("Pessoa não encontrada");

            try
            {
                contexto.Cidades.Remove(e);
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
