using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Data.Dto.CentroCusto;
using ApiGestaoFinanceira.Data.Dto.Lancamento;
using ApiGestaoFinanceira.Services;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiGestaoFinanceira.Controllers
{
    [Route("api/[controller]")]
    public class CentroCustosController : ControllerBase
    {
        private CentroCustoService _centroCustoService;

        public CentroCustosController(CentroCustoService centroCustoService)
        {
            _centroCustoService = centroCustoService;
        }

        [HttpPost]
        //[Authorize(Roles = "admin")]
        public IActionResult AdicionaCentroCusto([FromBody] CreateCentroCustoDto centroCustoDto)
        {
            ReadCentroCustoDto readDto = _centroCustoService.AdicionaCentroCusto(centroCustoDto);
            return CreatedAtAction(nameof(RecuperaCentroCustosPorId), new { Id = readDto.Id }, readDto);
        }

        /*[HttpGet]
        public IActionResult RecuperaCentroCustos()
        {
            List<ReadCentroCustoDto> readDto = _centroCustoService.RecuperaCentroCustos();
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }*/

        [HttpGet("{id}")]
        public IActionResult RecuperaCentroCustosPorId(int id)
        {
            ReadCentroCustoDto readDto = _centroCustoService.RecuperaCentroCustosPorId(id);
            if (readDto == null) return NotFound();
            return Ok(readDto);

        }

        [HttpPut("{id}")]
        public IActionResult AtualizaCentroCusto(int id, [FromBody] UpdateCentroCustoDto centroCustoDto)
        {
            Result resultado = _centroCustoService.AtualizaCentroCusto(id, centroCustoDto);
            if (resultado.IsFailed) return NotFound();
            return NoContent();
        }

        [HttpPut("del/{id}")]
        public IActionResult DeletaCentroCusto(int id, [FromBody] DeleteCentroCustoDto centroCustoDto)
        {
            Result resultado = _centroCustoService.DeletaCentroCusto(id, centroCustoDto);
            if (resultado.IsFailed) return NotFound();
            return NoContent();
        }

        [HttpGet("usuario/{idUsuario}")]
        public IActionResult RecuperaCentroCustosPorIdUsuario(int idUsuario)
        {
            List<ReadCentroCustoDto> readDto = _centroCustoService.RecuperaCentroCustosPorIdUsuario(idUsuario);
            if (readDto == null) return NotFound();
            return Ok(readDto);

        }

    }
}
