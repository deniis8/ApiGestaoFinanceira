using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Data.Dto.CentroCusto;
using ApiGestaoFinanceira.Data.Dto.DetalhamentoGastosCentroCusto;
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
    public class DetalhamentoGastosCentroCustoService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public DetalhamentoGastosCentroCustoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<ReadDetalhamentoGastosCentroCustoDto> RecuperaDetalhamentoGastosCentroCustoCC(string mesAno, string descCC)
        {
            List<DetalhamentoGastosCentroCusto> detalhamentoGastosCentroCusto;
            if(!string.IsNullOrEmpty(mesAno) && !string.IsNullOrEmpty(descCC)) {
                detalhamentoGastosCentroCusto = _context.DetalhamentoGastosCentroCustos.Where(cc => cc.MesAno == mesAno && cc.DescricaoCentroCusto == descCC).ToList();
            }else { 
                detalhamentoGastosCentroCusto = _context.DetalhamentoGastosCentroCustos.ToList();
            }

            if (detalhamentoGastosCentroCusto != null)
            {
                List<ReadDetalhamentoGastosCentroCustoDto> readDto = _mapper.Map<List<ReadDetalhamentoGastosCentroCustoDto>>(detalhamentoGastosCentroCusto);
                return readDto;
            }
            return null;
        }

    }
}
