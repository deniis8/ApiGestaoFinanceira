using ApiGestaoFinanceira.Data.Dto.GastosMensais;
using ApiGestaoFinanceira.Models;
using AutoMapper;

namespace ApiGestaoFinanceira.Profiles
{
    public class GastosCentroCustoProfile : Profile
    {
        public GastosCentroCustoProfile()
        {

            CreateMap<GastosCentroCusto, ReadGastosCentroCustoDto>();
        }
    }
}
