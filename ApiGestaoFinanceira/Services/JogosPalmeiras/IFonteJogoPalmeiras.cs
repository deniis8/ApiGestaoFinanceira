using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    public interface IFonteJogoPalmeiras
    {
        string Nome { get; }

        /// <summary>
        /// Retorna o jogo em andamento ou o próximo jogo do Palmeiras segundo esta fonte.
        /// Retorna null quando a fonte não tem jogo agendado e lança exceção quando não consegue consultar o site.
        /// </summary>
        Task<JogoEncontrado> BuscaProximoJogo();
    }
}
