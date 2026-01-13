using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.IA.GastosCentroCustoIA
{
    [Table("CONFIGURACOESIA")]
    public class ConfiguracoesIA
    {
        [Key]
        [Required]
        [Column("ID_CONFIGURACAO")]
        public int Id { get; set; }

        [Column("FILTRO_DATA_DE")]
        public DateTime FiltroDataDe { get; set; }

        [Column("FILTRO_DATA_ATE")]
        public DateTime FiltroDataAte { get; set; }

        [Column("PROMPT")]
        public string Prompt { get; set; }

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [Column("D_E_L_E_T_")]
        public char Deletado { get; set; }
    }
}