using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Data.Dto.CentroCusto;
using ApiGestaoFinanceira.Models;
using AutoMapper;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    public class CentroCustoService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public CentroCustoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ReadCentroCustoDto AdicionaCentroCusto(CreateCentroCustoDto centroCustoDto)
        {
            CentroCusto centroCusto = _mapper.Map<CentroCusto>(centroCustoDto);
            _context.CentroCustos.Add(centroCusto);
            _context.SaveChanges();
            return _mapper.Map<ReadCentroCustoDto>(centroCusto);
        }

        public List<ReadCentroCustoDto> RecuperaCentroCustos()
        {
            List<CentroCusto> centroCustos;
            centroCustos = _context.CentroCustos.ToList();

            if (centroCustos != null)
            {
                List<ReadCentroCustoDto> readDto = _mapper.Map<List<ReadCentroCustoDto>>(centroCustos);
                return readDto;
            }
            return null;
        }

        public ReadCentroCustoDto RecuperaCentroCustosPorId(int id)
        {
            CentroCusto centroCusto = _context.CentroCustos.FirstOrDefault(centroCusto => centroCusto.Id == id);
            if (centroCusto != null)
            {
                ReadCentroCustoDto centroCustoDto = _mapper.Map<ReadCentroCustoDto>(centroCusto);

                return centroCustoDto;
            }
            return null;
        }

        public Result AtualizaCentroCusto(int id, UpdateCentroCustoDto centroCustoDto)
        {
            CentroCusto centroCusto = _context.CentroCustos.FirstOrDefault(centroCusto => centroCusto.Id == id);
            if (centroCusto == null)
            {
                return Result.Fail("Centro de Custo não encontrado");
            }
            _mapper.Map(centroCustoDto, centroCusto);
            _context.SaveChanges();
            return Result.Ok();
        }

        public Result DeletaCentroCusto(int id)
        {
            CentroCusto centroCusto = _context.CentroCustos.FirstOrDefault(centroCusto => centroCusto.Id == id);
            if (centroCusto == null)
            {
                return Result.Fail("Centro de Custo não encontrado");
            }
            _context.Remove(centroCusto);
            _context.SaveChanges();
            return Result.Ok();
        }
    }
}
