using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Data.Dto.Lancamento;
using ApiGestaoFinanceira.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Profiles
{
    public class LancamentoFixoProfile : Profile
    {
        public LancamentoFixoProfile()
        {
            CreateMap<CreateLancamentoFixoDto, LancamentoFixo>();
            CreateMap<LancamentoFixo, ReadLancamentoFixoDto>();
            CreateMap<UpdateLancamentoFixoDto, LancamentoFixo>();
            CreateMap<DeleteLancamentoFixoDto, LancamentoFixo>();
        }
    }
}
