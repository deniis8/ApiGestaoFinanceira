using ApiGestaoFinanceira.Data.Dto.PainelFinanceiro;
using ApiGestaoFinanceira.Models.PainelFinanceiro;
using AutoMapper;

namespace ApiGestaoFinanceira.Profiles.PainelFinanceiro
{
    public class EvolucaoPatrimonioProfile : Profile
    {
        public EvolucaoPatrimonioProfile()
        {
            CreateMap<EvolucaoPatrimonio, ReadEvolucaoPatrimonioDto>();
        }
    }
}
