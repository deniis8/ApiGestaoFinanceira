using ApiGestaoFinanceira.Data.Dto.JogosPalmeiras;
using ApiGestaoFinanceira.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Controllers
{
    [Route("api/[controller]")]
    public class JogosPalmeirasController : ControllerBase
    {
        private ProximoJogoService _proximoJogoService;

        public JogosPalmeirasController(ProximoJogoService proximoJogoService)
        {
            _proximoJogoService = proximoJogoService;
        }

        [HttpGet("proximo")]
        [AllowAnonymous]
        public async Task<IActionResult> RecuperaProximoJogo()
        {
            try
            {
                ReadProximoJogoDto readDto = await _proximoJogoService.RecuperaProximoJogo();
                if (readDto == null) return NotFound();
                return Ok(readDto);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status502BadGateway, "Não foi possível obter a agenda do Palmeiras no momento.");
            }
        }
    }
}
