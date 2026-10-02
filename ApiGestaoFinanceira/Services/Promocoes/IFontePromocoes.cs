using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    public interface IFontePromocoes
    {
        string Nome { get; }

        /// <summary>
        /// Retorna as promoções atuais do tipo pedido segundo esta fonte (lista vazia quando não há nenhuma).
        /// Lança exceção quando não consegue consultar o site.
        /// </summary>
        Task<List<PromocaoEncontrada>> BuscaPromocoes(TipoPromocao tipo);
    }
}
