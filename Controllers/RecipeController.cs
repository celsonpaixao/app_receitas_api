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

        public async Task<ActionResult<DTOResponse>> Create([FromForm] RecipeModel receita, IFormFile image)
        {
            var response = await receitas.Create_Recipe(receita, image);

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
        [HttpGet("list_by_recipe_user")]

        [Authorize]
        public async Task<ActionResult<DTOResponse>> ListarReceitaPorId(int id)
        {
            var response = receitas.List_Recipe_By_User(id);
            return Ok(await response);
        }

        [Authorize]
        [HttpGet("list_by_recipe_category")]

        [Authorize]
        public async Task<ActionResult<DTOResponse>> ListarReceitaPorCategoria(int id)
        {
            var response = receitas.List_Recipe_By_Category(id);
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
        // [Authorize]
        [HttpPut("update_recipe")]


        public async Task<ActionResult<DTOResponse>> AtualizarReceita([FromForm] int id_receita, [FromForm] RecipeModel receita, IFormFile? image)
        {
            var response = await receitas.Update_Recipe(id_receita, receita, image);
            return Ok(response);
        }
    }
}