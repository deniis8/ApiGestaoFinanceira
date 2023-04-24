using System.Collections;
using Microsoft.AspNetCore.Mvc;
using ApiGestaoFinanceira.Services;

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

        [HttpGet()]
        public IActionResult getAllSaldosInvestimentos()
        {
            IEnumerable readDto = _saldosInvestimentosService.getSaldosInvestimentos();
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }
    }
}
