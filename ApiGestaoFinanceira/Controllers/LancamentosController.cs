using ApiGestaoFinanceira.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Controllers
{
    [Route("api/[controller]")]
    public class LancamentosController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public LancamentosController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetLancamentos()
        {
            return Ok(new
            {
                success = true,
                data = await _appDbContext.Lancamentos.ToListAsync()
            }
                );
        }
    }
}
