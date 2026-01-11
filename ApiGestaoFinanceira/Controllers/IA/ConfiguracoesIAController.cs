using ApiGestaoFinanceira.Data.Dto.IA.GastosCentroCustoIA;
using ApiGestaoFinanceira.Services;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiGestaoFinanceira.Controllers
{
    [Route("api/[controller]")]
    public class ConfiguracoesIAController : ControllerBase
    {
        private ConfiguracoesIAService _configuracoesIAService;

        public ConfiguracoesIAController(ConfiguracoesIAService configuracoesIAService)
        {
            _configuracoesIAService = configuracoesIAService;
        }

        [HttpPost]
        public IActionResult AdicionaConfiguracoesIA([FromBody] CreateConfiguracoesIADto configuracoesIADto)
        {
            ReadConfiguracoesIADto readDto = _configuracoesIAService.AdicionaConfiguracoesIA(configuracoesIADto);
            return CreatedAtAction(nameof(RecuperaConfiguracoesIAPorId), new { Id = readDto.Id }, readDto);
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaConfiguracoesIAPorId(int id)
        {
            ReadConfiguracoesIADto readDto = _configuracoesIAService.RecuperaConfiguracoesPorId(id);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaConfiguracoesIA(int id, [FromBody] UpdateConfiguracoesIADto configuracoesIADto)
        {
            Result resultado = _configuracoesIAService.AtualizaConfiguracoesIA(id, configuracoesIADto);
            if (resultado.IsFailed) return NotFound();
            return NoContent();
        }

        [HttpGet("usuario/{idUsuario}")]
        public IActionResult RecuperaConfiguracoesIAPorIdUsuario(int idUsuario)
        {
            ReadConfiguracoesIADto readDto = _configuracoesIAService.RecuperaConfiguracoesIAPorIdUsuario(idUsuario);
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }
    }
}