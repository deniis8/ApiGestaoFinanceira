using System.Text.Json.Serialization;

namespace ApiGestaoFinanceira.Services
{
    /// <summary>
    /// Linha da tabela public.partida_futebol no Supabase. Campos nulos não são enviados,
    /// então um upsert não apaga um valor que já esteja gravado.
    /// </summary>
    public class PartidaFutebol
    {
        [JsonPropertyName("jogo")]
        public string Jogo { get; set; }

        [JsonPropertyName("local")]
        public string Local { get; set; }

        /// <summary>ISO 8601 com fuso, ex.: 2026-09-20T11:00:00-03:00.</summary>
        [JsonPropertyName("data_hora")]
        public string DataHora { get; set; }

        /// <summary>Emissoras separadas por "|", ex.: "Globo|SporTV|Premiere".</summary>
        [JsonPropertyName("emissoras")]
        public string Emissoras { get; set; }
    }
}
