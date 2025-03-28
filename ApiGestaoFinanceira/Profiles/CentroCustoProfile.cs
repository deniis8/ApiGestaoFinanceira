﻿using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Data.Dto.CentroCusto;
using ApiGestaoFinanceira.Data.Dto.Lancamento;
using ApiGestaoFinanceira.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Profiles
{
    public class CentroCustoProfile : Profile
    {
        public CentroCustoProfile()
        {
            CreateMap<CreateCentroCustoDto, CentroCusto>();
            CreateMap<CentroCusto, ReadCentroCustoDto>();
            CreateMap<UpdateCentroCustoDto, CentroCusto>();
            CreateMap<DeleteCentroCustoDto, CentroCusto>();
        }
    }
}
