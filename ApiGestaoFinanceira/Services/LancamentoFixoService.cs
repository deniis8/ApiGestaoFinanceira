using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto.CentroCusto;
using ApiGestaoFinanceira.Data.Dto.Lancamento;
using ApiGestaoFinanceira.Models;
using AutoMapper;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiGestaoFinanceira.Services
{
    public class LancamentoFixoService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public LancamentoFixoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ReadLancamentoFixoDto AdicionaLancamentoFixo(CreateLancamentoFixoDto lancamentoFixoDto)
        {
            LancamentoFixo lancamentoFixo = _mapper.Map<LancamentoFixo>(lancamentoFixoDto);
            _context.LancamentosFixos.Add(lancamentoFixo);
            _context.SaveChanges();
            return _mapper.Map<ReadLancamentoFixoDto>(lancamentoFixo);
        }

        public List<ReadLancamentoFixoDto> RecuperaLancamentosFixosPorIdUsuario(int idUsuario)
        {
            List<LancamentoFixo> lancamentosFixos;
            lancamentosFixos = _context.LancamentosFixos.Where(cc => cc.IdUsuario == idUsuario && cc.Deletado.ToString() != "*").OrderBy(cc => cc.DiaMes).ToList();

            if (lancamentosFixos != null)
            {
                List<ReadLancamentoFixoDto> readDto = _mapper.Map<List<ReadLancamentoFixoDto>>(lancamentosFixos);
                return readDto;
            }
            return null;
        }

        public Object RecuperaLancamentosFixosPorId(int id)
        {
            LancamentoFixo lancamentoFixo;
            lancamentoFixo = _context.LancamentosFixos.FirstOrDefault(l => l.Id == id);

            if (lancamentoFixo != null)
            {
                ReadLancamentoFixoDto readDto = _mapper.Map<ReadLancamentoFixoDto>(lancamentoFixo);
                return readDto;
            }
            return null;
        }

        public Result AtualizaLancamentoFixo(int id, UpdateLancamentoFixoDto lancamentoFixoDto)
        {
            LancamentoFixo lancamentoFixo = _context.LancamentosFixos.FirstOrDefault(lancamentoFixo => lancamentoFixo.Id == id);
            if (lancamentoFixo == null)
            {
                return Result.Fail("Lançamento não encontrado");
            }
            _mapper.Map(lancamentoFixoDto, lancamentoFixo);
            _context.SaveChanges();
            return Result.Ok();
        }

        public Result DeletaLancamentoFixo(int id, DeleteLancamentoFixoDto lancamentoFixoDto)
        {
            LancamentoFixo lancamentoFixo = _context.LancamentosFixos.FirstOrDefault(lancamentoFixo => lancamentoFixo.Id == id);
            if (lancamentoFixo == null)
            {
                return Result.Fail("Lançamento não encontrado");
            }
            _mapper.Map(lancamentoFixoDto, lancamentoFixo);
            _context.SaveChanges();
            return Result.Ok();
        }
    }
}