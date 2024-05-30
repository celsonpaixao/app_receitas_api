using api_receita.DTO;
using app_receitas_api.Models;
using app_receitas_api.DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app_receitas_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipe receitas;
        public RecipeController(IRecipe _receitas)
        {
            receitas = _receitas;
        }
        [Authorize]
        [HttpPost("create_recipe")]

        public async Task<ActionResult<DTOResponse>> Create([FromForm] RecipeModel receita)
        {
            DTOResponse response = new DTOResponse();

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

                response = await receitas.Create_Recipe(receita);
                response.message = "Recipe create sucess!";
            }
            catch (Exception ex)
            {
                response.message = $"Opps we have a problem {ex.Message}";
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet("list_all_recipe")]

        public async Task<ActionResult<DTOResponse>> ListarTodasReceitas()
        {
            var response = receitas.List_Recipe();
            return Ok(await response);
        }
        [Authorize]
        [HttpGet("list_by_recipe_id")]

        // [Authorize]
        public async Task<ActionResult<DTOResponse>> ListarReceitaPorId(int id)
        {
            var response = receitas.List_Recipe_By_ID(id);
            return Ok(await response);
        }
        [Authorize]
        [HttpDelete("delete_recipe")]

        // [Authorize]
        public async Task<ActionResult<DTOResponse>> DeletarReceita(int id)
        {
            var response = receitas.Delete_Recipe(id);
            return Ok(await response);
        }
        [Authorize]
        [HttpPut("update_recipe")]


        public async Task<ActionResult<DTOResponse>> AtualizarReceita(int id_receita, RecipeModel receita)
        {
            var response = await receitas.Update_Recipe(id_receita, receita);

            return Ok(response);
        }
    }
}