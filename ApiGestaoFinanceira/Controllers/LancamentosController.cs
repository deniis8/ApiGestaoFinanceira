using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Services;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiGestaoFinanceira.Controllers
{
    [Route("api/[controller]")]
    public class LancamentosController : ControllerBase
    {
        private LancamentoService _lancamentoService;

        public LancamentosController(LancamentoService lancamentoService)
        {
            _lancamentoService = lancamentoService;
        }

        [HttpPost]
        //[Authorize(Roles = "admin")]
        public IActionResult AdicionaLancamento([FromBody] CreateLancamentoDto lancamentoDto)
        {
            ReadLancamentoDto readDto = _lancamentoService.AdicionaLancamento(lancamentoDto);
            return CreatedAtAction(nameof(RecuperaLancamentosPorId), new { Id = readDto.Id }, readDto);
        }

        [HttpGet]
        public IActionResult RecuperaLancamentos([FromQuery] int? classificacaoEtaria = null)
        {
            List<ReadLancamentoDto> readDto = _lancamentoService.RecuperaLancamentos();
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaLancamentosPorId(int id)
        {
            ReadLancamentoDto readDto = _lancamentoService.RecuperaLancamentosPorId(id);
            if (readDto == null) return NotFound();
            return Ok(readDto);

        }

        [HttpPut("{id}")]
        public IActionResult AtualizaLancamento(int id, [FromBody] UpdateLancamentoDto lancamentoDto)
        {
            Result resultado = _lancamentoService.AtualizaLancamento(id, lancamentoDto);
            if (resultado.IsFailed) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletaLancamento(int id)
        {
            Result resultado = _lancamentoService.DeletaLancamento(id);
            if (resultado.IsFailed) return NotFound();
            return NoContent();
        }

    }
    /*{
        private readonly AppDbContext _appDbContext;

        public LancamentosController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetLancamentos()
        {
            return Ok(new
            {
                success = true,
                data = await _appDbContext.Lancamentos.ToListAsync()
            }
                );
        }
    }*/
}
