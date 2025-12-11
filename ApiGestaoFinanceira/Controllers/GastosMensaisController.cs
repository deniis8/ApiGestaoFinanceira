using ApiGestaoFinanceira.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using ApiGestaoFinanceira.Data.Dto.GastosMensais;
using ApiGestaoFinanceira.Models;
using System.Threading.Tasks;

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

        [HttpGet("usuario/{idUsuario}/datade/{datade}")]
        public async Task<ActionResult<GastosMensais>> getGastosMensaisApartirDe(int idUsuario, string dataDe)
        {
            IEnumerable readDto = await _gastosMensaisService.getGastosMensaisApartirDe(idUsuario, dataDe);
            return Ok(readDto);
        }

        [HttpGet("usuario/{idUsuario}/datade/{datade}/dataate/{dataate}")]
        public async Task<ActionResult<GastosMensais>> getGastosMensaisApartirDeAte(int idUsuario, string dataDe, string dataAte)
        {
            IEnumerable readDto = await _gastosMensaisService.getGastosMensaisApartirDeAte(idUsuario, dataDe, dataAte);
            return Ok(readDto);
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<GastosMensais>> getAllGastosMensais(int idUsuario)
        {
            object readDto = await _gastosMensaisService.getGastosMensaisApartirDe(idUsuario, null);
            return Ok(readDto);
        }

    }
}
