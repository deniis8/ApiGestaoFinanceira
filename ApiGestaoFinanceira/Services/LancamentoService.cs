using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto.CentroCusto;
using ApiGestaoFinanceira.Data.Dto.DetalhamentoGastosCentroCusto;
using ApiGestaoFinanceira.Data.Dto.GastosCentroCusto;
using ApiGestaoFinanceira.Data.Dto.Lancamento;
using ApiGestaoFinanceira.Data.Dto.VWLancamento;
using ApiGestaoFinanceira.Models;
using AutoMapper;
using FluentResults;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

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

        public IEnumerable RecuperaLancamentosPorData(string data)
        {
            if (string.IsNullOrEmpty(data))
                data = Convert.ToString(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"));

            List<VWLancamento> vwLancamento;
            vwLancamento = _context.VWLancamentos.Where(l => l.DataHora >= DateTime.Parse(data)).ToList();

            if (vwLancamento != null)
            {
                List<ReadVWLancamentoDto> readDto = _mapper.Map<List<ReadVWLancamentoDto>>(vwLancamento);
                return readDto;
            }
            return null;
        }

        public Object RecuperaLancamentosPorId(int id)
        {
            VWLancamento vwLancamento;
            vwLancamento = _context.VWLancamentos.FirstOrDefault(l => l.Id == id);

            if (vwLancamento != null)
            {
                ReadVWLancamentoDto readDto = _mapper.Map<ReadVWLancamentoDto>(vwLancamento);
                return readDto;
            }
            return null;
        }

        public IEnumerable RecuperaLancamentosDataDeAte(string dataDe, string dataAte, string status, int idCentroCusto)
        {
            List<VWLancamento> vwLancamento;

            if ((dataDe != null && dataAte != null) & status == null)
            {
                vwLancamento = _context.VWLancamentos.Where(l => l.DataHora >= DateTime.Parse(dataDe + " 00:00") && l.DataHora <= DateTime.Parse(dataAte + " 23:59")).ToList();
                List<ReadVWLancamentoDto> readDto = _mapper.Map<List<ReadVWLancamentoDto>>(vwLancamento);
                return readDto;
            }
            else if((dataDe != null && dataAte != null) && status != null && idCentroCusto == 0)
            {
                vwLancamento = _context.VWLancamentos.Where(l => l.DataHora >= DateTime.Parse(dataDe + " 00:00") && l.DataHora <= DateTime.Parse(dataAte + " 23:59") && status.Contains(l.Status)).ToList();
                List<ReadVWLancamentoDto> readDto = _mapper.Map<List<ReadVWLancamentoDto>>(vwLancamento);
                return readDto;
            }
            else if ((dataDe != null && dataAte != null) && status != null && idCentroCusto > 0)
            {
                vwLancamento = _context.VWLancamentos.Where(l => l.DataHora >= DateTime.Parse(dataDe + " 00:00") && l.DataHora <= DateTime.Parse(dataAte + " 23:59") && status.Contains(l.Status) && l.IdCCusto == idCentroCusto).ToList();
                List<ReadVWLancamentoDto> readDto = _mapper.Map<List<ReadVWLancamentoDto>>(vwLancamento);
                return readDto;
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