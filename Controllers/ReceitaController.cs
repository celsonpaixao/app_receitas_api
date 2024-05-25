using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DTO;
using app_receitas_api.DAL.Interfaces;
using app_receitas_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace app_receitas_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceitaController : ControllerBase
    {
        private IReceitas receitas;
        public ReceitaController(IReceitas _receitas)
        {
            receitas = _receitas;
        }

        [HttpPost("create")]

        public async Task<ActionResult<DTOResposta>> Create(ReceitaModel receita)
        {

            var resposta = receitas.PublicarReceitas(receita);

            return Ok(await resposta);
        }

        [HttpGet("list_all")]

        public async Task<ActionResult<DTOResposta>> ListarTodasReceitas()
        {
            var resposta = receitas.ListarReceitas();
            return Ok(await resposta);
        }
    }
}