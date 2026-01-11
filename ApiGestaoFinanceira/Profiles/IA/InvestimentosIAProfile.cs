using ApiGestaoFinanceira.Data.Dto.DetalhamentoGastosCentroCusto;
using ApiGestaoFinanceira.Data.Dto.GastosMensais;
using ApiGestaoFinanceira.Data.Dto.IA.GastosCentroCustoIA;
using ApiGestaoFinanceira.Models;
using AutoMapper;

namespace ApiGestaoFinanceira.Profiles.IA
{
    public class InvestimentosIAProfile : Profile
    {
        public InvestimentosIAProfile()
        {

            CreateMap<InvestimentosIAProfile, ReadInvestimentosIADto>();
        }
    }
}
