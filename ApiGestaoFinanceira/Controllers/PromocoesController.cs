using ApiGestaoFinanceira.Data.Dto.Promocoes;
using ApiGestaoFinanceira.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Controllers
{
    [Route("api/[controller]")]
    public class PromocoesController : ControllerBase
    {
        private PromocoesService _promocoesService;

        public PromocoesController(PromocoesService promocoesService)
        {
            _promocoesService = promocoesService;
        }

        /// <summary>
        /// Ex.: GET api/Promocoes?tipo=tecnologia (tecnologia, moveis, tenis-e-roupas, viagens ou outros).
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RecuperaPromocoes([FromQuery] string tipo)
        {
            if (!TiposPromocao.TentaInterpretar(tipo, out var tipoPromocao))
                return BadRequest($"Informe o parâmetro tipo com um destes valores: {string.Join(", ", TiposPromocao.ValoresAceitos)}.");

            try
            {
                List<ReadPromocaoDto> readDto = await _promocoesService.RecuperaPromocoes(tipoPromocao);
                return Ok(readDto);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status502BadGateway, "Não foi possível obter as promoções no momento.");
            }
        }
    }
}
