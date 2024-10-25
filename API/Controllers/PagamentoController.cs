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
    public class PagamentoController : ControllerBase
    {
        private readonly IJWTAuthenticationManager jwtAuthenticationManager;

        public PagamentoController(IJWTAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpGet]
        [Route("pagamentos")]
        public async Task<IActionResult> getAllAsync(//consulta geral
           [FromServices] Contexto contexto)
        {
            var e = await contexto
                .Pagamentos
                .AsNoTracking()
                .ToListAsync();
            return e == null ? NotFound() : Ok(e);
        }


        [HttpGet]
        [Route("pagamentos/{periodo}")]
        public async Task<IActionResult> getByPeriodoAsync(//consulta por periodo
            [FromServices] Contexto contexto,
            [FromRoute] DateOnly periodo)
        {
            var pagamento = await contexto
                .Pagamentos
                .AsNoTracking()
                .Where(e => e.Periodo == periodo)
                .ToListAsync();

            return pagamento == null ? NotFound() : Ok(pagamento);
        }


        [HttpGet]
        [Route("pagamentos/{periodo}/{entregadorId}")]
        public async Task<IActionResult> getByPeriodoAndEntregadorAsync(
            [FromServices] Contexto contexto,
            [FromRoute] DateOnly periodo,
            [FromRoute] int entregadorId)
        {
            var pagamentos = await contexto
                .Pagamentos
                .AsNoTracking()
                .Where(e => e.Periodo == periodo && e.FkEntregador == entregadorId)
                .ToListAsync();

            return pagamentos == null || !pagamentos.Any() ? NotFound() : Ok(pagamentos);
        }


        [HttpPost]
        [Route("pagamentos")]
        public async Task<IActionResult> PostAsync(//cadastro
            [FromServices] Contexto contexto,
            [FromBody] Pagamento pagamento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await contexto.Pagamentos.AddAsync(pagamento);
                await contexto.SaveChangesAsync();
                return Created($"api/pagamentos/{pagamento.Id}", pagamento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("pagamentos/{id}")]
        public async Task<IActionResult> PutAsync(//editar
            [FromServices] Contexto contexto,
            [FromBody] Pagamento pagamento,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model inválida");
            var e = await contexto.Pagamentos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound("Pessoa não encontrada");

            try
            {
                e.Adicional = pagamento.Adicional;
                e.Desconto = pagamento.Desconto;
                e.Adiantamento = pagamento.Adiantamento;
                e.Pago = pagamento.Pago;
                e.NotaFiscal = pagamento.NotaFiscal;
                e.Periodo = pagamento.Periodo;

                contexto.Pagamentos.Update(e);
                await contexto.SaveChangesAsync();
                return Ok(e);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("pagamentos/{id}")]
        public async Task<IActionResult> DeleteAsync(//deletar
            [FromServices] Contexto contexto,
            [FromRoute] int id)
        {
            var e = await contexto.Pagamentos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound("Pessoa não encontrada");

            try
            {
                contexto.Pagamentos.Remove(e);
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
