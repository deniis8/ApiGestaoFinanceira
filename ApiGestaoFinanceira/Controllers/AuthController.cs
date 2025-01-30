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
        private readonly UsuarioService _usuarioService;
        private readonly TokenService _tokenService;

        public AuthController(UsuarioService usuarioService, TokenService tokenService)
        {
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] ReadUsuarioDto loginUsuarioDto)
        {
            ReadUsuarioDto usuario = _usuarioService.RecuperaUsuariosPorEmailESenha(loginUsuarioDto.Email, loginUsuarioDto.Senha);

            if (usuario == null)
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

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // Gera o refresh token
            var refreshToken = _tokenService.GenerateRefreshToken();
            _usuarioService.SalvaRefreshToken(usuario.Id, refreshToken, DateTime.Now.AddDays(7));

            return Ok(new { Token = accessToken, RefreshToken = refreshToken , idUsuario = usuario.Id});
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public IActionResult RefreshToken([FromBody] ReadUsuarioDto usuarioDto)
        {
            var usuario = _usuarioService.RecuperaUsuarioPorRefreshToken(usuarioDto.RefreshToken);

            if (usuario == null || usuario.RefreshTokenDataExpiracao <= DateTime.Now)
                return Unauthorized("Refresh token inválido ou expirado.");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Email),
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

            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            _usuarioService.SalvaRefreshToken(usuario.Id, newRefreshToken, DateTime.Now.AddDays(7));

            return Ok(new { Token = newAccessToken, RefreshToken = newRefreshToken, idUsuario = usuario.Id });
        }
    }
}