using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto.CentroCusto;
using ApiGestaoFinanceira.Data.Dto.Lancamento;
using ApiGestaoFinanceira.Models;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    public class LancamentosRecebidosIAService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public LancamentosRecebidosIAService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<LancamentosRecebidosIA>> RecuperaLancamentosRecebidosIA(int idUsuario, string dataDe, string dataAte)
        {
            var parameters = new[]
            {
                new MySqlParameter("@ID_USER", idUsuario),
                new MySqlParameter("@DATA_DE", dataDe),
                new MySqlParameter("@DATA_ATE", dataAte)
            };

            var lancamentosRecebidosIA = await _context.LancamentosRecebidosIA
                .FromSqlRaw("CALL SP_LANCAMENTOS_RECEBIDOS_IA(@ID_USER, @DATA_DE, @DATA_ATE)", parameters)
                .ToListAsync();

            return lancamentosRecebidosIA.AsEnumerable().ToList();
        }
    }
}