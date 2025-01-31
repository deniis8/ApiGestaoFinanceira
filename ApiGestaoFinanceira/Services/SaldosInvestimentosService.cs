using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto.Saldos;
using ApiGestaoFinanceira.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    public class SaldosInvestimentosService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public SaldosInvestimentosService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /*public Object getSaldosInvestimentos(int idUsuario)
        {
            List<SaldosInvestimentos> saldosInvestimentos;
            saldosInvestimentos = _context.SaldosInvestimentos.Where(s => s.IdUsuario == idUsuario).ToList();
            if (saldosInvestimentos.Any())
            {
                List<ReadSaldosInvestimentos> readDto = _mapper.Map<List<ReadSaldosInvestimentos>>(saldosInvestimentos);
                return readDto.FirstOrDefault();
            }
            return null;

        }*/

        public async Task<List<SaldosInvestimentos>> getSaldosInvestimentos(int idUsuario)
        {
            var parameters = new[]
            {
                new MySqlParameter("@ID_USER", idUsuario)
            };

            var saldosInvestimentos = await _context.SaldosInvestimentos
                .FromSqlRaw("CALL SP_SALDOS_INVESTIMENTOS(@ID_USER)", parameters)
                .ToListAsync();

            return saldosInvestimentos.AsEnumerable().ToList();
        }
    }
}
