using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Data.Dto.Usuario;
using ApiGestaoFinanceira.Models;
using AutoMapper;
using FluentResults;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    public class UsuarioService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public UsuarioService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ReadUsuarioDto AdicionaUsuario(CreateUsuarioDto usuarioDto)
        {
            Usuario usuario = _mapper.Map<Usuario>(usuarioDto);
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return _mapper.Map<ReadUsuarioDto>(usuario);
        }

        public List<ReadUsuarioDto> RecuperaUsuarios()
        {
            List<Usuario> usuarios;
            usuarios = _context.Usuarios.ToList();

            if (usuarios != null)
            {
                List<ReadUsuarioDto> readDto = _mapper.Map<List<ReadUsuarioDto>>(usuarios);
                return readDto;
            }
            return null;
        }

        public ReadUsuarioDto RecuperaUsuariosPorId(int id)
        {
            Usuario usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.Id == id);
            if (usuario != null)
            {
                ReadUsuarioDto usuarioDto = _mapper.Map<ReadUsuarioDto>(usuario);

                return usuarioDto;
            }
            return null;
        }

        public ReadUsuarioDto RecuperaUsuariosPorEmailESenha(string email, string senha)
        {
            Usuario usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.Email == email && usuario.Senha == senha);
            if (usuario != null)
            {
                ReadUsuarioDto usuarioDto = _mapper.Map<ReadUsuarioDto>(usuario);
                return usuarioDto;
            }
            return null;
        }

        public Result AtualizaUsuario(int id, UpdateUsuarioDto usuarioDto)
        {
            Usuario usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.Id == id);
            if (usuario == null)
            {
                return Result.Fail("Usuario não encontrado");
            }
            _mapper.Map(usuarioDto, usuario);
            _context.SaveChanges();
            return Result.Ok();
        }

        public Result DeletaUsuario(int id, DeleteUsuarioDto usuarioDto)
        {
            Usuario usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.Id == id);
            if (usuario == null)
            {
                return Result.Fail("Usuario não encontrado");
            }
            _mapper.Map(usuarioDto, usuario);
            _context.SaveChanges();
            return Result.Ok();
        }

        public ReadUsuarioDto RecuperaUsuarioPorRefreshToken(string refreshToken)
        {
            Usuario usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.RefreshToken == refreshToken);
            if (usuario != null)
            {
                ReadUsuarioDto usuarioDto = _mapper.Map<ReadUsuarioDto>(usuario);
                return usuarioDto;
            }
            return null;
        }

        public void SalvaRefreshToken(int userId, string refreshToken, DateTime expiryTime)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == userId);
            if (usuario != null)
            {
                usuario.RefreshToken = refreshToken;
                usuario.RefreshTokenDataExpiracao = expiryTime;
                _context.SaveChanges();
            }
        }
    }
}
