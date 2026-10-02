using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ApiGestaoFinanceira.Services
{
    public enum TipoPromocao
    {
        Tecnologia,
        Moveis,
        TenisERoupas,
        Viagens,
        Outros
    }

    public static class TiposPromocao
    {
        private static readonly Dictionary<TipoPromocao, string> Descricoes = new Dictionary<TipoPromocao, string>
        {
            [TipoPromocao.Tecnologia] = "Tecnologia",
            [TipoPromocao.Moveis] = "Móveis",
            [TipoPromocao.TenisERoupas] = "Tênis e Roupas",
            [TipoPromocao.Viagens] = "Viagens",
            [TipoPromocao.Outros] = "Outros"
        };

        // Chaves já normalizadas (sem acento, espaço ou hífen): "Tênis e Roupas", "tenis-e-roupas" e "TENIS E ROUPAS" caem no mesmo tipo.
        private static readonly Dictionary<string, TipoPromocao> Apelidos = new Dictionary<string, TipoPromocao>
        {
            ["tecnologia"] = TipoPromocao.Tecnologia,
            ["tech"] = TipoPromocao.Tecnologia,
            ["moveis"] = TipoPromocao.Moveis,
            ["movies"] = TipoPromocao.Moveis,
            ["teniseroupas"] = TipoPromocao.TenisERoupas,
            ["tenisroupas"] = TipoPromocao.TenisERoupas,
            ["tenis"] = TipoPromocao.TenisERoupas,
            ["roupas"] = TipoPromocao.TenisERoupas,
            ["moda"] = TipoPromocao.TenisERoupas,
            ["viagens"] = TipoPromocao.Viagens,
            ["viagem"] = TipoPromocao.Viagens,
            ["outros"] = TipoPromocao.Outros
        };

        /// <summary>Valores aceitos no parâmetro "tipo", para mensagens de erro e documentação.</summary>
        public static readonly string[] ValoresAceitos = { "tecnologia", "moveis", "tenis-e-roupas", "viagens", "outros" };

        public static string Descricao(TipoPromocao tipo) => Descricoes[tipo];

        public static bool TentaInterpretar(string texto, out TipoPromocao tipo)
        {
            tipo = default;
            return !string.IsNullOrWhiteSpace(texto) && Apelidos.TryGetValue(Normaliza(texto), out tipo);
        }

        private static string Normaliza(string texto)
        {
            var semAcento = texto.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark && char.IsLetterOrDigit(c));

            return new string(semAcento.ToArray()).ToLowerInvariant();
        }
    }
}
