using api_receita.DAL.Interfaces;
using api_receita.DTO;
using api_receita.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_receita.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRating avaliacao;

        public RatingController(IRating _avaliacao)
        {
            avaliacao = _avaliacao;
        }

        [HttpGet("list_all_avaliaction")]


        public async Task<ActionResult<DTOResponse>> ListarTodasAvaliacoes()
        {
            var resposta = await avaliacao.List_Rating();

            return Ok(resposta);
        }

        [HttpPost("public_avaliaction")]
        [Authorize]

        public async Task<ActionResult<DTOResponse>> AvaliarReceita(int id_receita, int id_user, RatingModel _avaliacao)
        {
            var resposta = await avaliacao.Create_Rating(id_receita, id_user, _avaliacao);

            return Ok(resposta);
        }

        [HttpDelete("delete_avaliaction")]
        [Authorize]
        public async Task<ActionResult<DTOResponse>> ApagarReceita(int id)
        {
            var resposta = await avaliacao.Delete_Rating(id);
            return Ok(resposta);
        }

        [HttpPut("update_avaliaction")]
        [Authorize]

        public async Task<ActionResult<DTOResponse>> AtualizarAvaliar(int id_avaliacao, RatingModel avaliacaoAtualizada)
        {
            var resposta = await avaliacao.Update_Rating(id_avaliacao, avaliacaoAtualizada);
            return Ok(resposta);
        }


    }
}