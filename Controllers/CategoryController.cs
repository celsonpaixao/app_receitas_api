using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DAL.Database;
using api_receita.DAL.Interfaces;
using api_receita.DTO;
using api_receita.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_receita.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory categoria;
        public CategoryController(ICategory _categoria)
        {
            categoria = _categoria;
        }

        [Authorize]
        [HttpGet("list_all_category")]
        public async Task<ActionResult<DTOResponse>> ListasTodasCategorias()
        {
            var resposta = await categoria.List_Category();
            if (resposta == null || resposta.statusCode != 200)
            {
                // Se houve um erro, retorna um BadRequest com a mensagem de erro
                return BadRequest(resposta?.message);
            }
            return Ok(resposta);
        }

        [HttpPost("create_category")]
        public async Task<ActionResult<DTOResponse>> CriarCategoria(CategoryModel _categoria)
        {
            var resposta = await categoria.Create_Category(_categoria);

            return Ok(resposta);
        }

        [HttpPut("update_category")]
        public async Task<ActionResult<DTOResponse>> AtualizarCategoria(int id, CategoryModel _categoria)
        {
            var resposta = await categoria.Upadate_Category(id, _categoria);

            return Ok(resposta);
        }

        [HttpDelete("delete_category")]
        public async Task<ActionResult<DTOResponse>> ApagarCategoria(int id)
        {
            var resposta = await categoria.Delete_Category(id);

            return Ok(resposta);
        }


    }
}