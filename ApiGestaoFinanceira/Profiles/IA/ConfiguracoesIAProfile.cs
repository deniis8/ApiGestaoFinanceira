using ApiGestaoFinanceira.Data.Dto.IA.GastosCentroCustoIA;
using AutoMapper;

namespace ApiGestaoFinanceira.Profiles.IA
{
    public class ConfiguracoesIAProfile : Profile
    {
        public ConfiguracoesIAProfile()
        {
            CreateMap<CreateConfiguracoesIADto, ConfiguracoesIA>();
            CreateMap<ConfiguracoesIA, ReadConfiguracoesIADto>();
            CreateMap<UpdateConfiguracoesIADto, ConfiguracoesIA>();
        }
    }
}