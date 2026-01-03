using ApiGestaoFinanceira.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using ApiGestaoFinanceira.Data.Dto.GastosMensais;
using ApiGestaoFinanceira.Models;
using System.Threading.Tasks;
using System;

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

        [HttpGet("")]
        public async Task<ActionResult<GastosMensais>> getGastosMensais([FromQuery] int idUsuario, [FromQuery] string dataDe, [FromQuery] string dataAte)
        {
            if (idUsuario == 0)
                return BadRequest("O parâmetro 'idUsuario' não está preenchido com uma informação válida.");

            if (string.IsNullOrEmpty(dataDe) && string.IsNullOrEmpty(dataAte))
            {
                dataDe = new DateTime(DateTime.Now.Year, 1, 1)
                .ToString("yyyy-MM-dd");

                dataAte = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else if (string.IsNullOrEmpty(dataDe) || string.IsNullOrEmpty(dataAte))
            {
                return BadRequest("Os parâmetros 'dataDe' e 'dataAté' precisam estar preenchidos os ambos sem preencher.");
            }

            IEnumerable readDto = await _gastosMensaisService.getGastosMensais(idUsuario, dataDe, dataAte);

            return Ok(readDto);
        }
    }
}