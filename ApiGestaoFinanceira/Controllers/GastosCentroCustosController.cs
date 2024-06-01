using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Data.Dto.CentroCusto;
using ApiGestaoFinanceira.Data.Dto.GastosMensais;
using ApiGestaoFinanceira.Data.Dto.Lancamento;
using ApiGestaoFinanceira.Services;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

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

        [HttpGet("data/{data}")]
        public IActionResult RecuperaCentroCustos(string data)
        {
            List<ReadGastosCentroCustoDto> readDto = _gastosCentroCustoService.RecuperaGastosCentroCusto(data);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

        [HttpGet("data")]
        public IActionResult RecuperaCentroCustosMesAnterior()
        {
            List<ReadGastosCentroCustoDto> readDto = _gastosCentroCustoService.RecuperaGastosCentroCusto(null);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

    }
}
