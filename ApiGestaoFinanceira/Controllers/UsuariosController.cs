using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Data.Dto.Usuario;
using ApiGestaoFinanceira.Services;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiGestaoFinanceira.Controllers
{
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        //[Authorize(Roles = "admin")]
        public IActionResult AdicionaUsuario([FromBody] CreateUsuarioDto usuarioDto)
        {
            ReadUsuarioDto readDto = _usuarioService.AdicionaUsuario(usuarioDto);
            return CreatedAtAction(nameof(RecuperaUsuariosPorId), new { Id = readDto.Id }, readDto);
        }

        [HttpGet]
        public IActionResult RecuperaUsuarios()
        {
            List<ReadUsuarioDto> readDto = _usuarioService.RecuperaUsuarios();
            if (readDto == null) return NotFound();
            return Ok(readDto);
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaUsuariosPorId(int id)
        {
            ReadUsuarioDto readDto = _usuarioService.RecuperaUsuariosPorId(id);
            if (readDto == null) return NotFound();
            return Ok(readDto);

        }

        [HttpPut("{id}")]
        public IActionResult AtualizaUsuario(int id, [FromBody] UpdateUsuarioDto usuarioDto)
        {
            Result resultado = _usuarioService.AtualizaUsuario(id, usuarioDto);
            if (resultado.IsFailed) return NotFound();
            return NoContent();
        }

        [HttpPut("del/{id}")]
        public IActionResult DeletaUsuario(int id, [FromBody] DeleteUsuarioDto usuarioDto)
        {
            Result resultado = _usuarioService.DeletaUsuario(id, usuarioDto);
            if (resultado.IsFailed) return NotFound();
            return NoContent();
        }
    }
}
