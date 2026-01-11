using ApiGestaoFinanceira.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.IA.GastosCentroCustoIA
{
    public class AnaliseFinanceiraIADto
    {
        public IEnumerable LancamentosRecebidos { get; set; }
        public IEnumerable GastosPorCentroDeCusto { get; set; }
        public IEnumerable Investimentos { get; set; }
        public object Saldos { get; set; }

    }
}
