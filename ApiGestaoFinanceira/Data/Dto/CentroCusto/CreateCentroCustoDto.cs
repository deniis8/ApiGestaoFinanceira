using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.CentroCusto
{
    public class CreateCentroCustoDto
    {
        [Required(ErrorMessage = "O campo Descrição do Centro de Custo é obrigatório")]
        [Column("DESCRI")]
        public string DescriCCusto { get; set; }

        [Required(ErrorMessage = "O campo Id Usuário é obrigatório")]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }
    }
}
