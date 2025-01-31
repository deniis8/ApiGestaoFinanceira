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

        [HttpGet("usuario/{idUsuario}/data/{data}")]
        public IActionResult getGastosMensaisApartirDe(int idUsuario, string data)
        {
            IEnumerable readDto = _gastosMensaisService.getGastosMensaisApartirDe(idUsuario, data);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

        [HttpGet("usuario/{idUsuario}")]
        public IActionResult getAllGastosMensais(int idUsuario)
        {
            object readDto = _gastosMensaisService.getGastosMensaisApartirDe(idUsuario, null);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

    }
}
