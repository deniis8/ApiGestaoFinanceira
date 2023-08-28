using ApiGestaoFinanceira.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using ApiGestaoFinanceira.Data.Dto.GastosMensais;

namespace ApiGestaoFinanceira.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GastosMensaisController : ControllerBase
    {
        private GastosMensaisService _gastosMensaisService;

        public GastosMensaisController(GastosMensaisService gastosMensaisService)
        {
            _gastosMensaisService = gastosMensaisService;
        }

        [HttpGet("{data}")]
        public IActionResult getGastosMensaisApartirDe(string data)
        {
            IEnumerable readDto = _gastosMensaisService.getGastosMensaisApartirDe(data);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

        [HttpGet()]
        public IActionResult getAllGastosMensais(string data)
        {
            object readDto = _gastosMensaisService.getGastosMensaisApartirDe(data);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

    }
}
