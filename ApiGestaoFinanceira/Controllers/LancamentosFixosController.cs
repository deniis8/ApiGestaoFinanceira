using ApiGestaoFinanceira.Data.Dto.Lancamento;
using ApiGestaoFinanceira.Services;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;

namespace ApiGestaoFinanceira.Controllers
{
    [Route("api/[controller]")]
    public class LancamentosFixosController : ControllerBase
    {
        private LancamentoFixoService _lancamentoFixoService;

        public LancamentosFixosController(LancamentoFixoService lancamentoFixoService)
        {
            _lancamentoFixoService = lancamentoFixoService;
        }

        [HttpPost]
        public IActionResult AdicionaLancamentoFixo([FromBody] CreateLancamentoFixoDto lancamentoFixoDto)
        {
            ReadLancamentoFixoDto readDto = _lancamentoFixoService.AdicionaLancamentoFixo(lancamentoFixoDto);
            return CreatedAtAction(nameof(RecuperaLancamentosFixosPorId), new { Id = readDto.Id }, readDto);
        }

        [HttpGet("usuario/{idUsuario}")]
        public IActionResult RecuperaLancamentosFixos(int idUsuario, string data)
        {
            IEnumerable readDto = _lancamentoFixoService.RecuperaLancamentosFixosPorIdUsuario(idUsuario);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaLancamentosFixosPorId(int id)
        {
            Object readDto = _lancamentoFixoService.RecuperaLancamentosFixosPorId(id);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaLancamentoFixo(int id, [FromBody] UpdateLancamentoFixoDto lancamentoFixoDto)
        {
            Result resultado = _lancamentoFixoService.AtualizaLancamentoFixo(id, lancamentoFixoDto);
            if (resultado.IsFailed) return NotFound();
            return NoContent();
        }

        [HttpPut("del/{id}")]
        public IActionResult DeletaLancamentoFixo(int id, [FromBody] DeleteLancamentoFixoDto lancamentoFixoDto)
        {
            Result resultado = _lancamentoFixoService.DeletaLancamentoFixo(id, lancamentoFixoDto);
            if (resultado.IsFailed) return NotFound();
            return NoContent();
        }

    }
}