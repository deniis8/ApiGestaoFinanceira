using ApiGestaoFinanceira.Data.Dto.PainelFinanceiro;
using ApiGestaoFinanceira.Models.PainelFinanceiro;
using AutoMapper;

namespace ApiGestaoFinanceira.Profiles.PainelFinanceiro
{
    public class ResumoMesProfile : Profile
    {
        public ResumoMesProfile()
        {
            CreateMap<ResumoMes, ReadResumoMesDto>();
        }
    }
}
