using ApiGestaoFinanceira.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class InvestimentosIAController : ControllerBase
    {
        private InvestimentosIAService _investimentosIAService;

        public InvestimentosIAController(InvestimentosIAService investimentosIAService)
        {
            _investimentosIAService = investimentosIAService;
        }

        [HttpGet("")]
        public async Task<ActionResult<InvestimentosIAController>> getInvestimentosIA([FromQuery] int idUsuario, [FromQuery] string dataDe, [FromQuery] string dataAte)
        {
            if (idUsuario == 0)
                return BadRequest("O parâmetro 'idUsuario' não está preenchido com uma informação válida.");           

            IEnumerable readDto = await _investimentosIAService.RecuperaInvestimentosIA(idUsuario, dataDe, dataAte);

            return Ok(readDto);
        }
    }
}