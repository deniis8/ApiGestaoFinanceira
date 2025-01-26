using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Data.Dto.Lancamento;
using ApiGestaoFinanceira.Data.Dto.VWLancamento;
using ApiGestaoFinanceira.Models;
using AutoMapper;

namespace ApiGestaoFinanceira.Profiles
{
    public class VWLancamentoProfile : Profile
    {
        public VWLancamentoProfile()
        {
            CreateMap<VWLancamento, ReadVWLancamentoDto>();
        }
    }
}
