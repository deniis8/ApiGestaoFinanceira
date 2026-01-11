using ApiGestaoFinanceira.Data.Dto.IA.GastosCentroCustoIA;
using ApiGestaoFinanceira.Services.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class AnaliseFinanceiraIAOpenAIService : IAnaliseFinanceiraIAOpenAIService
{
    private readonly HttpClient _httpClient;

    public AnaliseFinanceiraIAOpenAIService(
        HttpClient httpClient,
        IConfiguration configuration
    )
    {
        _httpClient = httpClient;

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                configuration["OpenAI:ApiKey"]
            );
    }

    public async Task<string> GerarAnaliseAsync(
        AnaliseFinanceiraIADto dados,
        string textoAuxiliar
    )
    {
        var prompt = MontarPrompt(dados, textoAuxiliar);

        var request = new
        {
            model = "gpt-4o-mini",
            input = new[]
            {
                new
                {
                    role = "user",
                    content = prompt
                }
            },
            max_output_tokens = 3000
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(
            "https://api.openai.com/v1/responses",
            content
        );

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        return doc.RootElement
            .GetProperty("output")[0]
            .GetProperty("content")[0]
            .GetProperty("text")
            .GetString();
    }

    private string MontarPrompt(
        AnaliseFinanceiraIADto dados,
        string textoAuxiliar
    )
    {
        return $"{textoAuxiliar}\n\nDados financeiros:\n{JsonSerializer.Serialize(dados)}";
    }
}
