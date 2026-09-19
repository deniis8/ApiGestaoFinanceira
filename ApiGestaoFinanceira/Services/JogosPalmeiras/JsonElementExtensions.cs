using System.Text.Json;

namespace ApiGestaoFinanceira.Services
{
    internal static class JsonElementExtensions
    {
        public static string TextoOuNulo(this JsonElement elemento, string propriedade)
        {
            return elemento.ValueKind == JsonValueKind.Object
                && elemento.TryGetProperty(propriedade, out var valor)
                && valor.ValueKind == JsonValueKind.String
                    ? valor.GetString()
                    : null;
        }
    }
}
