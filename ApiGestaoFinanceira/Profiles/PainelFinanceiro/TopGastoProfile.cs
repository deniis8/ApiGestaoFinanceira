using ApiGestaoFinanceira.Data.Dto.PainelFinanceiro;
using ApiGestaoFinanceira.Models.PainelFinanceiro;
using AutoMapper;

namespace ApiGestaoFinanceira.Profiles.PainelFinanceiro
{
    public class TopGastoProfile : Profile
    {
        public TopGastoProfile()
        {
            CreateMap<TopGasto, ReadTopGastoDto>();
        }
    }
}
