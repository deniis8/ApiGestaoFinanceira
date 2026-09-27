namespace ApiGestaoFinanceira.Data.Dto.PainelFinanceiro
{
    /// <summary>
    /// Nota de 0 a 100 sobre o mês, calculada em <see cref="Services.PainelFinanceiroService"/>:
    /// metade pela sobra em relação ao recebido, metade por quantos centros de custo ficaram
    /// dentro do limite (quando o usuário tem algum limite configurado).
    /// </summary>
    public class SaudeFinanceiraDto
    {
        public int Pontuacao { get; set; }

        /// <summary>"excelente" | "saudavel" | "atencao" | "alerta".</summary>
        public string Nivel { get; set; }

        public string Mensagem { get; set; }
    }
}
