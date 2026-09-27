using ApiGestaoFinanceira.Data.Dto.PainelFinanceiro;
using ApiGestaoFinanceira.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PainelFinanceiroController : ControllerBase
    {
        private readonly PainelFinanceiroService _painelFinanceiroService;

        public PainelFinanceiroController(PainelFinanceiroService painelFinanceiroService)
        {
            _painelFinanceiroService = painelFinanceiroService;
        }

        /// <summary>Saldo de fechamento mês a mês, para o gráfico de evolução do patrimônio.</summary>
        [HttpGet("usuario/{idUsuario}/evolucao")]
        public async Task<ActionResult<IEnumerable<ReadEvolucaoPatrimonioDto>>> GetEvolucaoPatrimonio(int idUsuario, [FromQuery] int? meses)
        {
            if (idUsuario <= 0)
                return BadRequest("O parâmetro 'idUsuario' não está preenchido com uma informação válida.");

            if (meses.HasValue && meses.Value <= 0)
                return BadRequest("O parâmetro 'meses' precisa ser maior que zero.");

            var resultado = await _painelFinanceiroService.GetEvolucaoPatrimonioAsync(idUsuario, meses);
            return Ok(resultado);
        }

        /// <summary>Resumo completo do mês selecionado na tela Gráficos (fixo x variável, maior
        /// gasto, ranking de gastos, padrão por dia da semana e a nota de saúde financeira).</summary>
        [HttpGet("usuario/{idUsuario}/mesAno/{mesAno}")]
        public async Task<ActionResult<ReadPainelMesDto>> GetPainelMes(int idUsuario, string mesAno)
        {
            if (idUsuario <= 0)
                return BadRequest("O parâmetro 'idUsuario' não está preenchido com uma informação válida.");

            if (string.IsNullOrWhiteSpace(mesAno))
                return BadRequest("O parâmetro 'mesAno' é obrigatório.");

            var resultado = await _painelFinanceiroService.GetPainelMesAsync(idUsuario, mesAno);
            return Ok(resultado);
        }
    }
}
