using System.Collections;
using Microsoft.AspNetCore.Mvc;
using ApiGestaoFinanceira.Services;
using System;

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
        public IActionResult getAllSaldosInvestimentos(int idUsuario)
        {
            Object readDto = _saldosInvestimentosService.getSaldosInvestimentos(idUsuario);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }
    }
}
