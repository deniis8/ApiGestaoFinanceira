using ApiGestaoFinanceira.Data.Dto.DetalhamentoGastosCentroCusto;
using ApiGestaoFinanceira.Data.Dto.GastosMensais;
using ApiGestaoFinanceira.Data.Dto.Lancamento;
using ApiGestaoFinanceira.Models;
using AutoMapper;

namespace ApiGestaoFinanceira.Profiles.IA
{
    public class LancamentosRecebidosIAProfile : Profile
    {
        public LancamentosRecebidosIAProfile()
        {

            CreateMap<LancamentosRecebidosIAProfile, ReadLancamentosRecebidosIADto>();
        }
    }
}
