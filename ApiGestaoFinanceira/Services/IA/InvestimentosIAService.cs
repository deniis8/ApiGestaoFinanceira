using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    public class InvestimentosIAService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public InvestimentosIAService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<InvestimentosIA>> RecuperaInvestimentosIA(int idUsuario, string dataDe, string dataAte)
        {
            var parameters = new[]
            {
                new MySqlParameter("@ID_USER", idUsuario),
                new MySqlParameter("@DATA_DE", dataDe),
                new MySqlParameter("@DATA_ATE", dataAte)
            };

            var investimentosIA = await _context.InvestimentosIA
                .FromSqlRaw("CALL SP_INVESTIMENTOS_POR_PERIODO_IA(@ID_USER, @DATA_DE, @DATA_ATE)", parameters)
                .ToListAsync();

            return investimentosIA.AsEnumerable().ToList();
        }
    }
}