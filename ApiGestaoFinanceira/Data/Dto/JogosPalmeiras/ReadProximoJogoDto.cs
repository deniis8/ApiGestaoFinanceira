using System.Collections.Generic;

namespace ApiGestaoFinanceira.Data.Dto.JogosPalmeiras
{
    public class ReadProximoJogoDto
    {
        public string Jogo { get; set; }
        public string Local { get; set; }
        public string DataHora { get; set; }
        public List<ReadEmissoraDto> OndeAssistir { get; set; }
    }

    public class ReadEmissoraDto
    {
        public string Emissora { get; set; }
    }
}
