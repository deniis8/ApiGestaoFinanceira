using ApiGestaoFinanceira.Data.Dto.IA.GastosCentroCustoIA;
using ApiGestaoFinanceira.Services;
using ApiGestaoFinanceira.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AnaliseFinanceiraIAController : ControllerBase
    {
        private LancamentosRecebidosIAService _lancamentosRecebidosIAService;
        private GastosCentroCustoIAService _gastosCentroCustoIAService;
        private InvestimentosIAService _investimentosIAService;
        private SaldosInvestimentosService _saldosInvestimentosService;
        private readonly IAnaliseFinanceiraIAOpenAIService _openAIService;

        public AnaliseFinanceiraIAController(LancamentosRecebidosIAService lancamentosRecebidosIAService,
            GastosCentroCustoIAService gastosCentroCustoIAService,
            InvestimentosIAService investimentosIAService,
            SaldosInvestimentosService saldosInvestimentosService,
            IAnaliseFinanceiraIAOpenAIService openAIService)
        {
            _lancamentosRecebidosIAService = lancamentosRecebidosIAService;
            _gastosCentroCustoIAService = gastosCentroCustoIAService;
            _investimentosIAService = investimentosIAService;
            _saldosInvestimentosService = saldosInvestimentosService;
            _openAIService = openAIService;
        }

        [HttpPost]
        public async Task<ActionResult> GerarAnalise([FromBody] ReadAnaliseFinanceiraIARequestDto request)
        {
            if (request.IdUsuario == 0)
                return BadRequest("O parâmetro 'idUsuario' não está preenchido com uma informação válida.");

            var lancamentosRecebidos = await _lancamentosRecebidosIAService
                .RecuperaLancamentosRecebidosIA(request.IdUsuario, request.DataDe, request.DataAte);

            var gastosCentroCusto = await _gastosCentroCustoIAService
                .RecuperaGastosCentroCustoIA(request.IdUsuario, request.DataDe, request.DataAte);

            var investimentos = await _investimentosIAService
                .RecuperaInvestimentosIA(request.IdUsuario, request.DataDe, request.DataAte);

            var saldos = await _saldosInvestimentosService
                .getSaldosInvestimentos(request.IdUsuario);

            var response = new ReadAnaliseFinanceiraIADto
            {
                LancamentosRecebidos = lancamentosRecebidos,
                GastosPorCentroDeCusto = gastosCentroCusto,
                Investimentos = investimentos,
                Saldos = saldos
            };

            var analiseIA = await _openAIService.GerarAnaliseAsync(response, request.TextoAuxiliar);

            return Ok(new
            {
                analiseIA
            });
        }
    }
}