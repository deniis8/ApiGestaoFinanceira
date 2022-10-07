using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto;
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
    public class LancamentoService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public LancamentoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ReadLancamentoDto AdicionaLancamento(CreateLancamentoDto lancamentoDto)
        {
            Lancamento lancamento = _mapper.Map<Lancamento>(lancamentoDto);
            _context.Lancamentos.Add(lancamento);
            _context.SaveChanges();
            return _mapper.Map<ReadLancamentoDto>(lancamento);
        }

        public IEnumerable RecuperaLancamentos()
        {
            List<Lancamento> lancamentos;
            lancamentos = _context.Lancamentos.ToList();

            //Centro de Custo
            List<CentroCusto> centroCustos;
            centroCustos = _context.CentroCustos.ToList();

            if (lancamentos != null)
            {
                List<ReadLancamentoDto> readLancamentoDto = _mapper.Map<List<ReadLancamentoDto>>(lancamentos);
                List<ReadCentroCustoDto> readCCustoDto = _mapper.Map<List<ReadCentroCustoDto>>(centroCustos);
                var resultado = from lanc in readLancamentoDto
                                join centroCusto in readCCustoDto
                                on lanc.IdCCusto equals centroCusto.Id
                                where lanc.Deletado != '*'
                                orderby lanc.DataHora descending
                                select new
                                {
                                    Id = lanc.Id,
                                    DataHora = lanc.DataHora,
                                    Valor = lanc.Valor,
                                    Descricao = lanc.Descricao,
                                    Status = lanc.Status,
                                    IdCCusto = lanc.IdCCusto,
                                    DescriCCusto = centroCusto.DescriCCusto
                                };
                return resultado;


            }
            return null;
        }

        public IEnumerable RecuperaLancamentosPorId(int id)
        {
            Lancamento lancamento = _context.Lancamentos.FirstOrDefault(lancamento => lancamento.Id == id);

            if (lancamento != null)
            {
                List<Lancamento> lancamentos;
                lancamentos = _context.Lancamentos.ToList();
                //Centro de Custo
                List<CentroCusto> centroCustos;
                centroCustos = _context.CentroCustos.ToList();

                List<ReadLancamentoDto> readLancamentoDto = _mapper.Map<List<ReadLancamentoDto>>(lancamentos);
                List<ReadCentroCustoDto> readCCustoDto = _mapper.Map<List<ReadCentroCustoDto>>(centroCustos);

                var resultado = from lanc in readLancamentoDto
                                join centroCusto in readCCustoDto
                                on lanc.IdCCusto equals centroCusto.Id
                                where lanc.Id == id && lanc.Deletado != '*'                               
                                select new
                                {
                                    Id = lanc.Id,
                                    DataHora = lanc.DataHora,
                                    Valor = lanc.Valor,
                                    Descricao = lanc.Descricao,
                                    Status = lanc.Status,
                                    IdCCusto = lanc.IdCCusto,
                                    DescriCCusto = centroCusto.DescriCCusto
                                };
                return resultado;
            }
            return null;            
        }

        public Result AtualizaLancamento(int id, UpdateLancamentoDto lancamentoDto)
        {
            Lancamento lancamento = _context.Lancamentos.FirstOrDefault(lancamento => lancamento.Id == id);
            if (lancamento == null)
            {
                return Result.Fail("Lançamento não encontrado");
            }
            _mapper.Map(lancamentoDto, lancamento);
            _context.SaveChanges();
            return Result.Ok();
        }

        /*public Result DeletaLancamento(int id)
        {
            Lancamento lancamento = _context.Lancamentos.FirstOrDefault(lancamento => lancamento.Id == id);
            if (lancamento == null)
            {
                return Result.Fail("Lançamento não encontrado");
            }
            _context.Remove(lancamento);
            _context.SaveChanges();
            return Result.Ok();
        }*/

        public Result DeletaLancamento(int id, DeleteLancamentoDto lancamentoDto)
        {
            Lancamento lancamento = _context.Lancamentos.FirstOrDefault(lancamento => lancamento.Id == id);
            if (lancamento == null)
            {
                return Result.Fail("Lançamento não encontrado");
            }
            _mapper.Map(lancamentoDto, lancamento);
            _context.SaveChanges();
            return Result.Ok();
        }
    }
}
