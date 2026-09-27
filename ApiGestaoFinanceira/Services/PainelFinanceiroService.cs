using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto.PainelFinanceiro;
using ApiGestaoFinanceira.Models.PainelFinanceiro;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    /// <summary>
    /// Alimenta o painel de indicadores da tela Gráficos: evolução do patrimônio (a partir da
    /// tabela SALDOS, já mantida pelos triggers de ATUALIZA_SALDOS) e o resumo do mês selecionado
    /// (fixo x variável, maior gasto, ranking de gastos, padrão por dia da semana e uma nota de
    /// saúde financeira calculada aqui, combinando os dois procedimentos abaixo).
    /// </summary>
    public class PainelFinanceiroService
    {
        private const decimal MetaTaxaPoupanca = 0.30m; // 30% de sobra sobre o recebido já vale a nota máxima nesse quesito

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PainelFinanceiroService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ReadEvolucaoPatrimonioDto>> GetEvolucaoPatrimonioAsync(int idUsuario, int? meses)
        {
            var parametros = new[]
            {
                new MySqlParameter("@ID_USER", idUsuario),
                new MySqlParameter("@MESES", (object)meses ?? DBNull.Value)
            };

            var evolucao = await _context.EvolucaoPatrimonio
                .FromSqlRaw("CALL SP_EVOLUCAO_PATRIMONIO(@ID_USER, @MESES)", parametros)
                .ToListAsync();

            return _mapper.Map<List<ReadEvolucaoPatrimonioDto>>(evolucao);
        }

        public async Task<ReadPainelMesDto> GetPainelMesAsync(int idUsuario, string mesAno)
        {
            // Chamadas sequenciais: o mesmo AppDbContext não suporta consultas em paralelo.
            ResumoMes resumo = await GetResumoMesAsync(idUsuario, mesAno);
            List<TopGasto> topGastos = await GetTopGastosAsync(idUsuario, mesAno);
            List<GastoPorDiaSemana> gastosPorDiaSemana = await GetGastosPorDiaSemanaAsync(idUsuario, mesAno);

            decimal sobraMes = resumo.ValorRecebidoMes - (resumo.ValorFixo + resumo.ValorVariavel);
            TopGasto maiorGasto = topGastos.FirstOrDefault(); // já vem ordenado por valor decrescente

            return new ReadPainelMesDto
            {
                MesAno = mesAno,
                ValorFixo = resumo.ValorFixo,
                ValorVariavel = resumo.ValorVariavel,
                QuantidadeFixa = resumo.QuantidadeFixa,
                QuantidadeVariavel = resumo.QuantidadeVariavel,
                TicketMedio = resumo.TicketMedio,
                ValorRecebidoMes = resumo.ValorRecebidoMes,
                SobraMes = sobraMes,
                MaiorGasto = maiorGasto != null
                    ? new MaiorGastoDto
                    {
                        Valor = maiorGasto.Valor,
                        Descricao = maiorGasto.Descricao,
                        DataHora = maiorGasto.DataHora,
                        CentroCusto = maiorGasto.DescricaoCentroCusto
                    }
                    : null,
                TotalCategoriasComLimite = resumo.TotalCategoriasComLimite,
                CategoriasEstouradas = resumo.CategoriasEstouradas,
                Saude = CalculaSaudeFinanceira(resumo.ValorRecebidoMes, sobraMes, resumo.TotalCategoriasComLimite, resumo.CategoriasEstouradas),
                TopGastos = _mapper.Map<List<ReadTopGastoDto>>(topGastos),
                GastosPorDiaSemana = _mapper.Map<List<ReadGastoPorDiaSemanaDto>>(gastosPorDiaSemana)
            };
        }

        private async Task<ResumoMes> GetResumoMesAsync(int idUsuario, string mesAno)
        {
            var parametros = new[]
            {
                new MySqlParameter("@ID_USER", idUsuario),
                new MySqlParameter("@MES_ANO", mesAno)
            };

            var resumo = await _context.ResumoMes
                .FromSqlRaw("CALL SP_RESUMO_MES(@ID_USER, @MES_ANO)", parametros)
                .ToListAsync();

            // A procedure sempre devolve uma linha (agregação sem GROUP BY), mas o fallback
            // evita uma exceção caso ela algum dia deixe de existir.
            return resumo.FirstOrDefault() ?? new ResumoMes();
        }

        private async Task<List<TopGasto>> GetTopGastosAsync(int idUsuario, string mesAno)
        {
            var parametros = new[]
            {
                new MySqlParameter("@ID_USER", idUsuario),
                new MySqlParameter("@MES_ANO", mesAno)
            };

            return await _context.TopGastos
                .FromSqlRaw("CALL SP_TOP_GASTOS_MES(@ID_USER, @MES_ANO)", parametros)
                .ToListAsync();
        }

        private async Task<List<GastoPorDiaSemana>> GetGastosPorDiaSemanaAsync(int idUsuario, string mesAno)
        {
            var parametros = new[]
            {
                new MySqlParameter("@ID_USER", idUsuario),
                new MySqlParameter("@MES_ANO", mesAno)
            };

            return await _context.GastosPorDiaSemana
                .FromSqlRaw("CALL SP_GASTOS_POR_DIA_SEMANA(@ID_USER, @MES_ANO)", parametros)
                .ToListAsync();
        }

        /// <summary>
        /// Nota de 0 a 100: metade pela sobra do mês em relação ao recebido (30% de sobra já
        /// vale nota máxima nesse quesito), metade por quantos centros de custo com limite
        /// definido ficaram dentro dele. Sem nenhum limite configurado, a nota vem só da sobra.
        /// </summary>
        private static SaudeFinanceiraDto CalculaSaudeFinanceira(decimal valorRecebidoMes, decimal sobraMes, int totalCategoriasComLimite, int categoriasEstouradas)
        {
            decimal taxaPoupanca = valorRecebidoMes > 0 ? sobraMes / valorRecebidoMes : 0m;
            decimal fatorPoupanca = Math.Max(0m, Math.Min(1m, taxaPoupanca / MetaTaxaPoupanca));

            int pontuacao;
            if (totalCategoriasComLimite > 0)
            {
                decimal fatorLimites = 1m - ((decimal)categoriasEstouradas / totalCategoriasComLimite);
                pontuacao = (int)Math.Round(fatorPoupanca * 50m + fatorLimites * 50m);
            }
            else
            {
                pontuacao = (int)Math.Round(fatorPoupanca * 100m);
            }
            pontuacao = Math.Max(0, Math.Min(100, pontuacao));

            string nivel;
            string mensagem;
            if (pontuacao >= 80)
            {
                nivel = "excelente";
                mensagem = "Sobrou bastante do que entrou e os limites estão sob controle.";
            }
            else if (pontuacao >= 60)
            {
                nivel = "saudavel";
                mensagem = "O mês fechou de forma equilibrada, sem grandes desvios.";
            }
            else if (pontuacao >= 40)
            {
                nivel = "atencao";
                mensagem = "A sobra encolheu ou algum limite passou do combinado — vale revisar os gastos.";
            }
            else
            {
                nivel = "alerta";
                mensagem = "Os gastos pesaram bastante neste mês: pouca sobra e limites estourados.";
            }

            return new SaudeFinanceiraDto { Pontuacao = pontuacao, Nivel = nivel, Mensagem = mensagem };
        }
    }
}
