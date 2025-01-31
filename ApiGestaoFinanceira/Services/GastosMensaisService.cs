using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Data.Dto.GastosMensais;
using ApiGestaoFinanceira.Models;
using AutoMapper;
using FluentResults;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ApiGestaoFinanceira.Services
{
    public class GastosMensaisService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public GastosMensaisService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable getGastosMensaisApartirDe(int idUsuario, string data)
        {
            if (string.IsNullOrEmpty(data))
                data = Convert.ToString(DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd"));

            List<GastosMensais> gastosMensais;
            gastosMensais = _context.GastosMensais.ToList();

            if (gastosMensais != null)
            {
                List<ReadGastosMensaisDto> readGastosMensaisDto = _mapper.Map<List<ReadGastosMensaisDto>>(gastosMensais);
                var resultado = from gastM in readGastosMensaisDto
                                where gastM.IdUsuario == idUsuario && gastM.DataHora >= DateTime.Parse(data)
                                orderby gastM.DataHora ascending
                                select new
                                {
                                    Valor = gastM.Valor,
                                    Ano = gastM.Ano,
                                    Mes = gastM.Mes,
                                    DataHora = gastM.DataHora,
                                    SobraMes = gastM.SobraMes,
                                    ValorRecebidoMes = gastM.ValorRecebidoMes,
                                    IdUsuario = gastM.IdUsuario
                                };
                return resultado;


            }
            return null;
        }
    }
}
