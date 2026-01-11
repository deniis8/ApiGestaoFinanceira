using ApiGestaoFinanceira.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.IA.GastosCentroCustoIA
{
    public class AnaliseFinanceiraIARequestDto
    {
        public int IdUsuario { get; set; }
        public string DataDe { get; set; }
        public string DataAte { get; set; }
        public string TextoAuxiliar { get; set; }

    }
}
