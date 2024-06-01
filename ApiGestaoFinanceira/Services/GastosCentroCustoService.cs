using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Data.Dto.CentroCusto;
using ApiGestaoFinanceira.Data.Dto.GastosMensais;
using ApiGestaoFinanceira.Data.Dto.Lancamento;
using ApiGestaoFinanceira.Models;
using AutoMapper;
using FluentResults;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ApiGestaoFinanceira.Services
{
    public class GastosCentroCustoService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public GastosCentroCustoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<ReadGastosCentroCustoDto> RecuperaGastosCentroCusto(string data)
        {
            if (string.IsNullOrEmpty(data))
                data = Convert.ToString(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"));

            List<GastosCentroCusto> gastosCentroCusto;
            gastosCentroCusto = _context.GastosCentroCustos.Where(cc => cc.DataHora >= DateTime.Parse(data)).ToList();

            if (gastosCentroCusto != null)
            {
                List<ReadGastosCentroCustoDto> readDto = _mapper.Map<List<ReadGastosCentroCustoDto>>(gastosCentroCusto);
                return readDto;
            }
            return null;
        }

    }
}
