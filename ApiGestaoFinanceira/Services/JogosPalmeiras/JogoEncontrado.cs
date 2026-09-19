using System;
using System.Collections.Generic;

namespace ApiGestaoFinanceira.Services
{
    /// <summary>
    /// Jogo do Palmeiras como uma fonte de dados o enxerga, antes de ser consolidado com as demais.
    /// </summary>
    public class JogoEncontrado
    {
        public const string Palmeiras = "Palmeiras";

        public string Oponente { get; set; }
        public string Local { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan? Hora { get; set; }
        public List<string> Emissoras { get; set; } = new List<string>();
    }
}
