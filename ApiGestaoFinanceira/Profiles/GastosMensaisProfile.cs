using ApiGestaoFinanceira.Data.Dto.GastosMensais;
using ApiGestaoFinanceira.Models;
using AutoMapper;

namespace ApiGestaoFinanceira.Profiles
{
    public class GastosMensaisProfile : Profile
    {
        public GastosMensaisProfile()
        {

            CreateMap<GastosMensais, ReadGastosMensaisDto>();
        }
    }
}
