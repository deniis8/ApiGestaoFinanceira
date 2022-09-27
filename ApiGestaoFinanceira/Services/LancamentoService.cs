using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Models;
using AutoMapper;
using FluentResults;
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

        public List<ReadLancamentoDto> RecuperaLancamentos()
        {
            List<Lancamento> lancamentos;
            lancamentos = _context.Lancamentos.ToList();

            if (lancamentos != null)
            {
                List<ReadLancamentoDto> readDto = _mapper.Map<List<ReadLancamentoDto>>(lancamentos);
                return readDto;
            }
            return null;
        }

        public ReadLancamentoDto RecuperaLancamentosPorId(int id)
        {
            Lancamento lancamento = _context.Lancamentos.FirstOrDefault(lancamento => lancamento.Id == id);
            if (lancamento != null)
            {
                ReadLancamentoDto lancamentoDto = _mapper.Map<ReadLancamentoDto>(lancamento);

                return lancamentoDto;
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

        public Result DeletaLancamento(int id)
        {
            Lancamento lancamento = _context.Lancamentos.FirstOrDefault(lancamento => lancamento.Id == id);
            if (lancamento == null)
            {
                return Result.Fail("Lançamento não encontrado");
            }
            _context.Remove(lancamento);
            _context.SaveChanges();
            return Result.Ok();
        }
    }
}
