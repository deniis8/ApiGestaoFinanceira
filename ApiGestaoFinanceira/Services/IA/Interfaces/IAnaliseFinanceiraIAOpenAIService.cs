using ApiGestaoFinanceira.Data.Dto.IA.GastosCentroCustoIA;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services.Interfaces
{
    public interface IAnaliseFinanceiraIAOpenAIService
    {
        Task<string> GerarAnaliseAsync(AnaliseFinanceiraIADto dados, string textoAuxiliar);
    }
}
