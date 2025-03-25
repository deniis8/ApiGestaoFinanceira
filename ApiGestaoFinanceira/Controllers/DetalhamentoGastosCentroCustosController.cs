using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Data.Dto.CentroCusto;
using ApiGestaoFinanceira.Data.Dto.DetalhamentoGastosCentroCusto;
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
    public class DetalhamentoGastosCentroCustosController : ControllerBase
    {
        private DetalhamentoGastosCentroCustoService _detalhamentoGastosCentroCustoService;

        public DetalhamentoGastosCentroCustosController(DetalhamentoGastosCentroCustoService detalhamentoGastosCentroCustoService)
        {
            _detalhamentoGastosCentroCustoService = detalhamentoGastosCentroCustoService;
        }

        /*[HttpGet("descCC")]
        public IActionResult RecuperaDetalhamentoCentroCustos()
        {
            List<ReadDetalhamentoGastosCentroCustoDto> readDto = _detalhamentoGastosCentroCustoService.RecuperaDetalhamentoGastosCentroCustoCC(null, null);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }*/

        [HttpGet("descricaoCC")]
        public IActionResult RecuperaCentroCustosMesAno([FromQuery] int idUsuario, string mesAno, string descCC)
        {
            List<ReadDetalhamentoGastosCentroCustoDto> readDto = _detalhamentoGastosCentroCustoService.RecuperaDetalhamentoGastosCentroCustoCC(idUsuario, mesAno, descCC);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }       

    }
}
