namespace ApiGestaoFinanceira.Data.Dto.IA.GastosCentroCustoIA
{
    public class ReadAnaliseFinanceiraIARequestDto
    {
        public int IdUsuario { get; set; }
        public string DataDe { get; set; }
        public string DataAte { get; set; }
        public string TextoAuxiliar { get; set; }
    }
}