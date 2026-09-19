using ApiGestaoFinanceira.Data.Dto.JogosPalmeiras;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    public class ProximoJogoService
    {
        private const string CacheKey = "proximo-jogo-palmeiras";

        private static readonly TimeSpan TempoCache = TimeSpan.FromMinutes(30);
        // Se alguma fonte falhou, o resultado pode estar incompleto: guarda menos tempo para tentar de novo logo.
        private static readonly TimeSpan TempoCacheParcial = TimeSpan.FromMinutes(5);

        private readonly IEnumerable<IFonteJogoPalmeiras> _fontes;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ProximoJogoService> _logger;

        public ProximoJogoService(IEnumerable<IFonteJogoPalmeiras> fontes, IMemoryCache cache, ILogger<ProximoJogoService> logger)
        {
            _fontes = fontes;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Consulta todas as fontes e devolve o próximo jogo do Palmeiras com a união das emissoras encontradas.
        /// Retorna null quando nenhuma fonte tem jogo agendado e lança InvalidOperationException quando todas falham.
        /// </summary>
        public async Task<ReadProximoJogoDto> RecuperaProximoJogo()
        {
            if (_cache.TryGetValue(CacheKey, out ReadProximoJogoDto emCache))
                return emCache;

            var consultas = await Task.WhenAll(_fontes.Select(ConsultaFonte));

            var falhas = consultas.Count(c => c.Falhou);
            if (falhas == consultas.Length)
                throw new InvalidOperationException("Nenhuma fonte de agenda do Palmeiras respondeu.");

            var encontrados = consultas.Where(c => c.Jogo != null).Select(c => c.Jogo).ToList();
            if (!encontrados.Any()) return null;

            var jogo = MontaDto(Consolida(encontrados));
            _cache.Set(CacheKey, jogo, falhas > 0 ? TempoCacheParcial : TempoCache);
            return jogo;
        }

        private async Task<(JogoEncontrado Jogo, bool Falhou)> ConsultaFonte(IFonteJogoPalmeiras fonte)
        {
            try
            {
                return (await fonte.BuscaProximoJogo(), false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha ao consultar a agenda do Palmeiras em {Fonte}", fonte.Nome);
                return (null, true);
            }
        }

        /// <summary>
        /// A primeira fonte (a de maior prioridade) define qual é o jogo; as demais só entram se falarem do mesmo dia.
        /// Campos que a principal não tem são completados pelas outras, e as emissoras são unidas.
        /// </summary>
        private static JogoEncontrado Consolida(List<JogoEncontrado> encontrados)
        {
            var principal = encontrados[0];
            var mesmoJogo = encontrados.Where(j => j.Data.Date == principal.Data.Date).ToList();

            return new JogoEncontrado
            {
                Data = principal.Data,
                Hora = mesmoJogo.Select(j => j.Hora).FirstOrDefault(h => h != null),
                Local = mesmoJogo.Select(j => j.Local).FirstOrDefault(l => !string.IsNullOrWhiteSpace(l)),
                Oponente = mesmoJogo.Select(j => j.Oponente).FirstOrDefault(o => !string.IsNullOrWhiteSpace(o)),
                Emissoras = UneEmissoras(mesmoJogo)
            };
        }

        private static List<string> UneEmissoras(List<JogoEncontrado> jogos)
        {
            var vistas = new HashSet<string>();
            var emissoras = new List<string>();

            foreach (var nome in jogos.SelectMany(j => j.Emissoras))
            {
                if (vistas.Add(Normaliza(nome)))
                    emissoras.Add(nome.Trim());
            }
            return emissoras;
        }

        // "Cazé TV", "CazéTV" e "cazetv" contam como a mesma emissora.
        private static string Normaliza(string nome)
        {
            var semAcento = nome.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark && char.IsLetterOrDigit(c));

            return new string(semAcento.ToArray()).ToLowerInvariant();
        }

        private static ReadProximoJogoDto MontaDto(JogoEncontrado jogo)
        {
            var data = jogo.Data.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            return new ReadProximoJogoDto
            {
                Jogo = string.IsNullOrWhiteSpace(jogo.Oponente)
                    ? JogoEncontrado.Palmeiras
                    : $"{JogoEncontrado.Palmeiras} X {jogo.Oponente}",
                Local = jogo.Local,
                DataHora = jogo.Hora.HasValue ? $"{data} {jogo.Hora.Value:hh\\:mm\\:ss}" : data,
                OndeAssistir = jogo.Emissoras.Select(e => new ReadEmissoraDto { Emissora = e }).ToList()
            };
        }
    }
}
