using ApiGestaoFinanceira.Data.Dto.IA.GastosCentroCustoIA;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services.Interfaces
{
    public interface IAnaliseFinanceiraIAOpenAIService
    {
        Task<string> GerarAnaliseAsync(ReadAnaliseFinanceiraIADto dados, string textoAuxiliar);
    }
}
