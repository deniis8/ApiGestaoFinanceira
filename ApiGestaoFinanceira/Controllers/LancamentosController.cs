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

        [HttpGet("usuario/{idUsuario}/data/{data}")]
        public IActionResult RecuperaLancamentos(int idUsuario, string data)
        {
            IEnumerable readDto = _lancamentoService.RecuperaLancamentosPorData(idUsuario, data);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

        [HttpGet("usuario/{idUsuario}")]
        public IActionResult RecuperaLancamentosMesAnterior(int idUsuario)
        {
            IEnumerable readDto = _lancamentoService.RecuperaLancamentosPorData(idUsuario, null);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

        [HttpGet("dataDeAte")]
        public IActionResult RecuperaLancamentosDeAte([FromQuery] int idUsuario, string dataDe, string dataAte, string status, int idCentroCusto)
        {
            IEnumerable readDto = _lancamentoService.RecuperaLancamentosDataDeAte(idUsuario, dataDe, dataAte, status, idCentroCusto);
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

        [HttpGet("usuario/{idUsuario}/idCentroCusto/{idCentroCusto}")]
        public IActionResult ExisteCentroCustoParaLancamento(int idUsuario, int idCentroCusto)
        {
            int quantidade = _lancamentoService.RecuperaCentroCustoParaLancamento(idUsuario, idCentroCusto);
            return Ok(new { quantidade });
        }

    }
}