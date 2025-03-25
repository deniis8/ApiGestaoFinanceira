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

        [HttpGet("usuario/{idUsuario}/data/{data}")]
        public async Task<ActionResult<GastosMensais>> getGastosMensaisApartirDe(int idUsuario, string data)
        {
            IEnumerable readDto = await _gastosMensaisService.getGastosMensaisApartirDe(idUsuario, data);
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
