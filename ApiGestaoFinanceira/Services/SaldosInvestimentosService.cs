using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto.Saldos;
using ApiGestaoFinanceira.Models;
using AutoMapper;
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

        public List<ReadSaldosInvestimentos> getSaldosInvestimentos()
        {
            List<SaldosInvestimentos> saldosInvestimentos;
            saldosInvestimentos = _context.SaldosInvestimentos.ToList();
            if (saldosInvestimentos != null)
            {
                List<ReadSaldosInvestimentos> readDto = _mapper.Map<List<ReadSaldosInvestimentos>>(saldosInvestimentos);
                return readDto;
            }
            return null;

        }
    }
}
