namespace ApiGestaoFinanceira.Services
{
    /// <summary>
    /// Seção "Supabase" do appsettings. A ServiceRoleKey é secreta: informe por variável de ambiente
    /// (Supabase__ServiceRoleKey) e nunca a versione.
    /// </summary>
    public class SupabaseOptions
    {
        public string Url { get; set; }
        public string ServiceRoleKey { get; set; }
        public string TabelaPartidas { get; set; } = "partida_futebol";

        public bool Configurado => !string.IsNullOrWhiteSpace(Url) && !string.IsNullOrWhiteSpace(ServiceRoleKey);
    }

    /// <summary>
    /// Seção "SincronizacaoJogos" do appsettings.
    /// </summary>
    public class SincronizacaoJogosOptions
    {
        /// <summary>Horários (HH:mm, hora local do servidor) das sincronizações diárias. Padrão: 07:00, 13:00 e 19:00.</summary>
        public string[] Horarios { get; set; }

        /// <summary>Sincroniza também assim que a API sobe, para a tabela não ficar vazia até o primeiro horário.</summary>
        public bool ExecutarNaInicializacao { get; set; } = true;
    }
}
