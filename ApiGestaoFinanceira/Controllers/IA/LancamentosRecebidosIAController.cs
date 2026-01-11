using ApiGestaoFinanceira.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LancamentosRecebidosIAController : ControllerBase
    {
        private LancamentosRecebidosIAService _lancamentosRecebidosIAService;

        public LancamentosRecebidosIAController(LancamentosRecebidosIAService lancamentosRecebidosIAService)
        {
            _lancamentosRecebidosIAService = lancamentosRecebidosIAService;
        }

        [HttpGet("")]
        public async Task<ActionResult<LancamentosRecebidosIAController>> getLancamentosRecebidosIA([FromQuery] int idUsuario, [FromQuery] string dataDe, [FromQuery] string dataAte)
        {
            if (idUsuario == 0)
                return BadRequest("O parâmetro 'idUsuario' não está preenchido com uma informação válida.");           

            IEnumerable readDto = await _lancamentosRecebidosIAService.RecuperaLancamentosRecebidosIA(idUsuario, dataDe, dataAte);

            return Ok(readDto);
        }
    }
}