using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Services;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpGet]
        public IActionResult RecuperaCentroCustos([FromQuery] int? classificacaoEtaria = null)
        {
            List<ReadCentroCustoDto> readDto = _centroCustoService.RecuperaCentroCustos();
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

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

        [HttpDelete("{id}")]
        public IActionResult DeletaCentroCusto(int id)
        {
            Result resultado = _centroCustoService.DeletaCentroCusto(id);
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
