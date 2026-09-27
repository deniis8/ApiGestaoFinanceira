using ApiGestaoFinanceira.Data.Dto.PainelFinanceiro;
using ApiGestaoFinanceira.Models.PainelFinanceiro;
using AutoMapper;

namespace ApiGestaoFinanceira.Profiles.PainelFinanceiro
{
    public class GastoPorDiaSemanaProfile : Profile
    {
        public GastoPorDiaSemanaProfile()
        {
            CreateMap<GastoPorDiaSemana, ReadGastoPorDiaSemanaDto>();
        }
    }
}
