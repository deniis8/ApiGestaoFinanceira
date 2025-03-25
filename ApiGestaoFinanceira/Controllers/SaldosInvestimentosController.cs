using Microsoft.AspNetCore.Mvc;
using ApiGestaoFinanceira.Services;
using System;
using ApiGestaoFinanceira.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Controllers
{
    [Route("api/[controller]")]
    public class SaldosInvestimentosController : ControllerBase
    {
        private SaldosInvestimentosService _saldosInvestimentosService;

        public SaldosInvestimentosController(SaldosInvestimentosService saldosInvestimentosService)
        {
            _saldosInvestimentosService = saldosInvestimentosService;
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<SaldosInvestimentos>> GetSaldoInvestimento(int idUsuario)
        {
            var readDto  = await _saldosInvestimentosService.getSaldosInvestimentos(idUsuario);
            return Ok(readDto);
        }
    }
}
