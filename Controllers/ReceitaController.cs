using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DTO;
using app_receitas_api.DAL.Interfaces;
using app_receitas_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app_receitas_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceitaController : ControllerBase
    {
        private readonly IReceitas receitas;
        public ReceitaController(IReceitas _receitas)
        {
            receitas = _receitas;
        }
        [Authorize]
        [HttpPost("create_recive")]
      
        public async Task<ActionResult<DTOResposta>> Create([FromForm] ReceitaModel receita)
        {
            DTOResposta resposta = new DTOResposta();

            try
            {
                if (receita.ImagePash != null)
                {
                    var imgDirectory = Path.Combine("Uploads", "Receita_Img");
                    if (!Directory.Exists(imgDirectory))
                    {
                        Directory.CreateDirectory(imgDirectory);
                    }

                    var imgPath = Path.Combine(imgDirectory, receita.ImagePash.FileName);

                    using (var fileStream = new FileStream(imgPath, FileMode.Create))
                    {
                        await receita.ImagePash.CopyToAsync(fileStream);
                    }

                    receita.ImageURL = imgPath; // Atualizando o caminho da imagem no modelo
                }

                resposta = await receitas.PublicarReceitas(receita);
                resposta.mensagem = "Receita publicada com sucesso!";
            }
            catch (Exception ex)
            {
                resposta.mensagem = $"Erro ao publicar a receita: {ex.Message}";
            }

            return Ok(resposta);
        }

        [Authorize]
        [HttpGet("list_all_recive")]
        
        public async Task<ActionResult<DTOResposta>> ListarTodasReceitas()
        {
            var resposta = receitas.ListarReceitas();
            return Ok(await resposta);
        }
        [Authorize]
        [HttpGet("list_by_recive_id")]
       
        // [Authorize]
        public async Task<ActionResult<DTOResposta>> ListarReceitaPorId(int id)
        {
            var resposta = receitas.ListarPorID(id);
            return Ok(await resposta);
        }
        [Authorize]
        [HttpDelete("delete_recive")]
     
        // [Authorize]
        public async Task<ActionResult<DTOResposta>> DeletarReceita(int id)
        {
            var resposta = receitas.PagarReceita(id);
            return Ok(await resposta);
        }
        [Authorize]
        [HttpPut("update_recive")]
       

        public async Task<ActionResult<DTOResposta>> AtualizarReceita(int id_receita, ReceitaModel receita)
        {
            var resposta = receitas.AtualizarReceitas(id_receita, receita);

            return Ok(resposta);
        }
    }
}