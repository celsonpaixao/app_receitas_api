using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DAL.Interfaces;
using api_receita.DTO;
using api_receita.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_receita.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacao avaliacao;

        public AvaliacaoController(IAvaliacao _avaliacao)
        {
            avaliacao = _avaliacao;
        }

        [HttpGet("list_all_avaliaction")]
      

        public async Task<ActionResult<DTOResposta>> ListarTodasAvaliacoes()
        {
            var resposta = await avaliacao.ListarAvaliacao();

            return Ok(resposta);
        }

        [HttpPost("public_avaliaction")]
        [Authorize]

        public async Task<ActionResult<DTOResposta>> AvaliarReceita(int id_receita, int id_user, AvaliacaoModel _avaliacao)
        {
            var resposta = await avaliacao.CadastrarAvaliar(id_receita,id_user, _avaliacao);

            return Ok(resposta);
        }

        [HttpDelete("delete_avaliaction")]
        [Authorize]
        public async Task<ActionResult<DTOResposta>> ApagarReceita(int id)
        {
            var resposta = await avaliacao.ApagarAvaliacao(id);
            return Ok(resposta);
        }

        [HttpPut("update_avaliaction")]
        [Authorize]

        public async Task<ActionResult<DTOResposta>> AtualizarAvaliar(int id_avaliacao, AvaliacaoModel avaliacaoAtualizada)
        {
            var resposta = await avaliacao.AtualizarAvaliar(id_avaliacao, avaliacaoAtualizada);
            return Ok(resposta);
        }


    }
}