using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    /// <summary>
    /// Grava partidas no Supabase pela API REST (PostgREST), sem depender de SDK.
    /// Usa a service_role key, que ignora o RLS: só este servidor escreve na tabela.
    /// </summary>
    public class PartidaFutebolSupabaseRepositorio
    {
        // Chave do upsert: a mesma UNIQUE (jogo, data_hora) criada na tabela.
        private const string ColunasConflito = "jogo,data_hora";

        private static readonly JsonSerializerOptions JsonOpcoes = new JsonSerializerOptions { IgnoreNullValues = true };

        private readonly HttpClient _httpClient;
        private readonly SupabaseOptions _opcoes;

        public PartidaFutebolSupabaseRepositorio(HttpClient httpClient, IOptions<SupabaseOptions> opcoes)
        {
            _httpClient = httpClient;
            _opcoes = opcoes.Value;
        }

        public async Task Upsert(PartidaFutebol partida, CancellationToken cancellationToken = default)
        {
            if (!_opcoes.Configurado)
                throw new InvalidOperationException("Supabase não configurado: informe Supabase:Url e Supabase:ServiceRoleKey.");

            // Aceita tanto a URL do projeto (https://xxx.supabase.co) quanto a rota completa (.../rest/v1/partida_futebol).
            if (!Uri.TryCreate(_opcoes.Url.Trim(), UriKind.Absolute, out var uri) || (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp))
                throw new InvalidOperationException("Supabase:Url inválida: use algo como https://SEU-PROJETO.supabase.co");

            var url = $"{uri.GetLeftPart(UriPartial.Authority)}/rest/v1/{_opcoes.TabelaPartidas}?on_conflict={ColunasConflito}";
            var chave = _opcoes.ServiceRoleKey.Trim();

            using var requisicao = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(partida, JsonOpcoes), Encoding.UTF8, "application/json")
            };
            requisicao.Headers.Add("apikey", chave);
            // Chaves no formato antigo (JWT: anon/service_role) também vão em Authorization.
            // As novas (sb_secret_...) não são JWT e só vão no header apikey.
            if (chave.StartsWith("eyJ", StringComparison.Ordinal))
                requisicao.Headers.Authorization = new AuthenticationHeaderValue("Bearer", chave);
            requisicao.Headers.Add("Prefer", "resolution=merge-duplicates,return=minimal");

            using var resposta = await _httpClient.SendAsync(requisicao, cancellationToken);
            if (resposta.IsSuccessStatusCode) return;

            var corpo = await resposta.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Supabase respondeu {(int)resposta.StatusCode} ao gravar a partida: {corpo}");
        }
    }
}
