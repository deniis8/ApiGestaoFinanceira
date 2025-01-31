using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Data.Dto.CentroCusto;
using ApiGestaoFinanceira.Data.Dto.GastosCentroCusto;
using ApiGestaoFinanceira.Data.Dto.GastosMensais;
using ApiGestaoFinanceira.Data.Dto.Lancamento;
using ApiGestaoFinanceira.Models;
using ApiGestaoFinanceira.Services;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Controllers
{
    [Route("api/[controller]")]
    public class GastosCentroCustosController : ControllerBase
    {
        private GastosCentroCustoService _gastosCentroCustoService;

        public GastosCentroCustosController(GastosCentroCustoService gastosCentroCustoService)
        {
            _gastosCentroCustoService = gastosCentroCustoService;
        }

        [HttpGet("data")]
        public IActionResult RecuperaCentroCustosMesAnterior()
        {
            List<ReadGastosCentroCustoDto> readDto = _gastosCentroCustoService.RecuperaGastosCentroCusto(null);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

        [HttpGet("data/{data}")]
        public IActionResult RecuperaCentroCustos(string data)
        {
            List<ReadGastosCentroCustoDto> readDto = _gastosCentroCustoService.RecuperaGastosCentroCusto(data);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

        [HttpGet("usuario/{idUsuario}/mesAno/{mesAno}")]
        public async Task<ActionResult<IEnumerable<Lancamento>>> GetLancamentos(int idUsuario, string mesAno)
        {
            var lancamentos = await _gastosCentroCustoService.RecuperaGastosCentroCustoMesAno(idUsuario, mesAno);
            return Ok(lancamentos);
        }

    }
}
