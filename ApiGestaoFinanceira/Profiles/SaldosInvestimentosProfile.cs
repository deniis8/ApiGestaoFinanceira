using ApiGestaoFinanceira.Data.Dto.Saldos;
using ApiGestaoFinanceira.Models;
using AutoMapper;

namespace ApiGestaoFinanceira.Profiles
{
    public class SaldosInvestimentosProfile : Profile
    {
        public SaldosInvestimentosProfile()
        {
            CreateMap <SaldosInvestimentos, ReadSaldosInvestimentos>();
        }
    }
}
