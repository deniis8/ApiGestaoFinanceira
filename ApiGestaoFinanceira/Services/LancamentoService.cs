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

        public IEnumerable<ReadVWLancamentoDto> RecuperaLancamentosPorData(int idUsuario, string data)
        {
            DateTime dataFiltro;

            if (string.IsNullOrEmpty(data))
            {
                var ultimoPagamento = _context.VWLancamentos
                    .Where(l => l.IdUsuario == idUsuario && l.DescriCCusto == "Pagamento" && l.Status == "Recebido")
                    .OrderByDescending(l => l.DataHora)
                    .FirstOrDefault();

                dataFiltro = ultimoPagamento?.DataHora ?? DateTime.Now.AddMonths(-1);
            }
            else
            {
                dataFiltro = DateTime.Parse(data);
            }

            var vwLancamento = _context.VWLancamentos
                .Where(l => l.IdUsuario == idUsuario && l.DataHora >= dataFiltro)
                .ToList();

            return _mapper.Map<List<ReadVWLancamentoDto>>(vwLancamento);
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

        public IEnumerable RecuperaLancamentosDataDeAte(int idUsuario, string dataDe, string dataAte, string status, string idCentroCusto)
        {
            var query = _context.VWLancamentos.Where(l =>
                l.IdUsuario == idUsuario &&
                l.DataHora >= DateTime.Parse(dataDe + " 00:00") &&
                l.DataHora <= DateTime.Parse(dataAte + " 23:59") &&
                status.Contains(l.Status)
            );

            if (!string.IsNullOrWhiteSpace(idCentroCusto) && idCentroCusto != "0")
            {
                var listaIds = idCentroCusto
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x.Trim()))
                    .ToList();

                query = query.Where(l => listaIds.Contains(l.IdCCusto));
            }

            var vwLancamento = query.ToList();

            return _mapper.Map<List<ReadVWLancamentoDto>>(vwLancamento);
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

        public int RecuperaCentroCustoParaLancamento(int idUsuario, int idCentroCusto)
        {
            // Contando o número de registros encontrados
            var quantidade = _context.VWLancamentos
                                      .Where(l => l.IdUsuario == idUsuario && l.IdCCusto == idCentroCusto)
                                      .Count(); // Contando os registros que atendem à condição

            return quantidade; // Retorna o número de registros encontrados
        }
    }
}