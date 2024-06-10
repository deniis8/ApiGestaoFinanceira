using ApiGestaoFinanceira.Data.Dto.DetalhamentoGastosCentroCusto;
using ApiGestaoFinanceira.Data.Dto.GastosMensais;
using ApiGestaoFinanceira.Models;
using AutoMapper;

namespace ApiGestaoFinanceira.Profiles
{
    public class DetalhamentoGastosCentroCustoProfile : Profile
    {
        public DetalhamentoGastosCentroCustoProfile()
        {

            CreateMap<DetalhamentoGastosCentroCusto, ReadDetalhamentoGastosCentroCustoDto>();
        }
    }
}
