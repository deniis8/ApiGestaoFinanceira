using ApiGestaoFinanceira.Data.Dto.DetalhamentoGastosCentroCusto;
using ApiGestaoFinanceira.Data.Dto.GastosMensais;
using ApiGestaoFinanceira.Data.Dto.IA.GastosCentroCustoIA;
using ApiGestaoFinanceira.Models;
using AutoMapper;

namespace ApiGestaoFinanceira.Profiles.IA
{
    public class GastosCentroCustoIAProfile : Profile
    {
        public GastosCentroCustoIAProfile()
        {

            CreateMap<GastosCentroCustoIAProfile, ReadGastosCentroCustoIADto>();
        }
    }
}
