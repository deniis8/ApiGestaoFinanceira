using ApiGestaoFinanceira.Data.Dto.Usuario;
using ApiGestaoFinanceira.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiGestaoFinanceira.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private UsuarioService _usuarioService;
        public AuthController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] ReadUsuarioDto loginUsuarioDto)
        {
            bool usuarioExiste = _usuarioService.RecuperaUsuariosPorEmailESenha(loginUsuarioDto.Email, loginUsuarioDto.Senha);
            
            if(!usuarioExiste)
                return Unauthorized("Usuário ou senha inválidos.");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, loginUsuarioDto.Email),
                new Claim(ClaimTypes.Role, "admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aplicacaogestaofinanceira@123"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "gestao-financeira",
                audience: "clientes-gestao",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}

