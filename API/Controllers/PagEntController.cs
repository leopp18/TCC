using API_TCC.Data;
using API_TCC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API_TCC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagEntController : ControllerBase
    {
        private readonly IJWTAuthenticationManager jwtAuthenticationManager;

        public PagEntController(IJWTAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpGet]
        [Route("pagamentoentrega")]
        public async Task<IActionResult> getAllAsync(//consulta geral
            [FromServices] Contexto contexto)
        {
            var e = await contexto
                .PagamentoEntregas
                .AsNoTracking()
                .ToListAsync();
            return e == null ? NotFound() : Ok(e);
        }

        [HttpGet]
        [Route("pagamentoentrega/{id}")]
        public async Task<IActionResult> getByIdAsync(//consulta por id
            [FromServices] Contexto contexto,
            [FromRoute] int id)
        {
            var pagamentoEntrega = await contexto
                .PagamentoEntregas
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            return pagamentoEntrega == null ? NotFound() : Ok(pagamentoEntrega);
        }

        [HttpPost]
        [Route("pagamentoentrega")]
        public async Task<IActionResult> PostAsync(//cadastro
            [FromServices] Contexto contexto,
            [FromBody] PagamentoEntrega pagamentoEntrega)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await contexto.PagamentoEntregas.AddAsync(pagamentoEntrega);
                await contexto.SaveChangesAsync();
                return Created($"api/pagamentoentrega/{pagamentoEntrega.Id}", pagamentoEntrega);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("pagamentoentrega/{id}")]
        public async Task<IActionResult> PutAsync(//editar
            [FromServices] Contexto contexto,
            [FromBody] PagamentoEntrega pagamentoEntrega,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model inválida");
            var e = await contexto.PagamentoEntregas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound("PagamentoEntrega não encontrado");

            try
            {
                e.FkEntrega = pagamentoEntrega.FkEntrega;
                e.FkPagamento = pagamentoEntrega.FkPagamento;
                e.Quantidade = pagamentoEntrega.Quantidade;

                contexto.PagamentoEntregas.Update(e);
                await contexto.SaveChangesAsync();
                return Ok(e);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("pagamentoentrega/{id}")]
        public async Task<IActionResult> DeleteAsync(//deletar
            [FromServices] Contexto contexto,
            [FromRoute] int id)
        {
            var e = await contexto.PagamentoEntregas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound("PagamentoEntrega não encontrado");

            try
            {
                contexto.PagamentoEntregas.Remove(e);
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
