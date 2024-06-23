using ApiGestaoFinanceira.Data.Dto.Lancamento;
using ApiGestaoFinanceira.Services;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;

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
        public IActionResult AdicionaLancamento([FromBody] CreateLancamentoDto lancamentoDto)
        {
            ReadLancamentoDto readDto = _lancamentoService.AdicionaLancamento(lancamentoDto);
            return CreatedAtAction(nameof(RecuperaLancamentosPorId), new { Id = readDto.Id }, readDto);
        }

        [HttpGet("data/{data}")]
        public IActionResult RecuperaLancamentos(string data)
        {
            IEnumerable readDto = _lancamentoService.RecuperaLancamentosPorData(data);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

        [HttpGet("data")]
        public IActionResult RecuperaLancamentosMesAnterior()
        {
            IEnumerable readDto = _lancamentoService.RecuperaLancamentosPorData(null);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

        [HttpGet("dataDeAte")]
        public IActionResult RecuperaLancamentosDeAte([FromQuery] string dataDe, string dataAte)
        {
            IEnumerable readDto = _lancamentoService.RecuperaLancamentosDataDeAte(dataDe, dataAte);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaLancamentosPorId(int id)
        {
            Object readDto = _lancamentoService.RecuperaLancamentosPorId(id);
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

        [HttpPut("del/{id}")]
        public IActionResult DeletaLancamento(int id, [FromBody] DeleteLancamentoDto lancamentoDto)
        {
            Result resultado = _lancamentoService.DeletaLancamento(id, lancamentoDto);
            if (resultado.IsFailed) return NotFound();
            return NoContent();
        }
    }
}