using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Profiles
{
    public class LancamentoProfile : Profile
    {
        public LancamentoProfile()
        {
            CreateMap<CreateLancamentoDto, Lancamento>();
            CreateMap<Lancamento, ReadLancamentoDto>();
            CreateMap<UpdateLancamentoDto, Lancamento>();
        }
    }
}
