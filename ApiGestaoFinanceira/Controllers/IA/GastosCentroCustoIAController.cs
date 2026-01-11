using ApiGestaoFinanceira.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GastosCentroCustoIAController : ControllerBase
    {
        private GastosCentroCustoIAService _gastosCentroCustoIAService;

        public GastosCentroCustoIAController(GastosCentroCustoIAService gastosCentroCustoIAService)
        {
            _gastosCentroCustoIAService = gastosCentroCustoIAService;
        }

        [HttpGet("")]
        public async Task<ActionResult<GastosCentroCustoIAController>> getGastosCentroCustoIA([FromQuery] int idUsuario, [FromQuery] string dataDe, [FromQuery] string dataAte)
        {
            if (idUsuario == 0)
                return BadRequest("O parâmetro 'idUsuario' não está preenchido com uma informação válida.");           

            IEnumerable readDto = await _gastosCentroCustoIAService.RecuperaGastosCentroCustoIA(idUsuario, dataDe, dataAte);

            return Ok(readDto);
        }
    }
}