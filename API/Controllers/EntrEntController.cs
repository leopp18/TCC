using API_TCC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_TCC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EntrEntController : ControllerBase
    {
        private readonly IJWTAuthenticationManager jwtAuthenticationManager;

        public EntrEntController(IJWTAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpGet]
        [Route("entregador_entrega")]
        public async Task<IActionResult> getAllAsync(//consulta geral
            [FromServices] Contexto contexto)
        {
            var e = await contexto
                .EntregadorEntregas
                .AsNoTracking()
                .ToListAsync();
            return e == null ? NotFound() : Ok(e);
        }

        [HttpGet]
        [Route("entregador_entrega/{fkEntregador}/{fkEntrega}")]
        public async Task<IActionResult> getByFkEntregadorAndFkEntregaAsync(//consulta por fkEntregador e fkEntrega
        [FromServices] Contexto contexto,
        [FromRoute] int fkEntregador,
        [FromRoute] int fkEntrega)
        {
            var entregadorEntregas = await contexto
                .EntregadorEntregas
                .AsNoTracking()
                .Where(e => e.FkEntregador == fkEntregador && e.FkEntrega == fkEntrega)
                .ToListAsync();

            return entregadorEntregas == null || !entregadorEntregas.Any() ? NotFound() : Ok(entregadorEntregas);
        }

        [HttpGet]
        [Route("entregador_entrega/{fkEntregador}")]
        public async Task<IActionResult> getByFkEntregadorAsync(
        [FromServices] Contexto contexto,
        [FromRoute] int fkEntregador)
        {
            var entregadorEntregas = await contexto
                .EntregadorEntregas
                .AsNoTracking()
                .Include(e => e.FkEntregaNavigation) // Inclui a navegação para a tabela Entrega
                .Where(e => e.FkEntregador == fkEntregador)
                .ToListAsync();

            return entregadorEntregas == null || !entregadorEntregas.Any() ? NotFound() : Ok(entregadorEntregas);
        }

        [HttpPost]
        [Route("entregador_entrega")]
        public async Task<IActionResult> PostAsync(//cadastro
            [FromServices] Contexto contexto,
            [FromBody] Models.EntregadorEntrega entregadorEntrega)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await contexto.EntregadorEntregas.AddAsync(entregadorEntrega);
                await contexto.SaveChangesAsync();
                return Created($"api/entregador_entrega/{entregadorEntrega.Id}", entregadorEntrega);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("entregador_entrega/{id}")]
        public async Task<IActionResult> PutAsync(//editar
            [FromServices] Contexto contexto,
            [FromBody] Models.EntregadorEntrega entregadorEntrega,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model inválida");
            var e = await contexto.EntregadorEntregas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound("EntregadorEntrega não encontrado");

            try
            {
                e.FkEntregador = entregadorEntrega.FkEntregador;
                e.FkEntrega = entregadorEntrega.FkEntrega;

                contexto.EntregadorEntregas.Update(e);
                await contexto.SaveChangesAsync();
                return Ok(e);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("entregador_entrega/{id}")]
        public async Task<IActionResult> DeleteAsync(//deletar
            [FromServices] Contexto contexto,
            [FromRoute] int id)
        {
            var e = await contexto.EntregadorEntregas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound("EntregadorEntrega não encontrado");

            try
            {
                contexto.EntregadorEntregas.Remove(e);
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
