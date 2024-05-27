using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DAL.Database;
using api_receita.DAL.Interfaces;
using api_receita.DTO;
using api_receita.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_receita.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoria categoria;
        public CategoriaController(ICategoria _categoria)
        {
            categoria = _categoria;
        }

        [HttpGet("list_all_category")]
        public async Task<ActionResult<DTOResposta>> ListasTodasCategorias()
        {
            var resposta = await categoria.ListaCategoria();
            return Ok(resposta);
        }

        [HttpPost("create_category")]
        public async Task<ActionResult<DTOResposta>> CriarCategoria(CategoriaModel _categoria)
        {
            var resposta = await categoria.CadastrarCategoria(_categoria);

            return Ok(resposta);
        }

        [HttpPut("update_category")]
        public async Task<ActionResult<DTOResposta>> AtualizarCategoria(int id, CategoriaModel _categoria)
        {
            var resposta = await categoria.AtualizarCategoria(id, _categoria);

            return Ok(resposta);
        }

        [HttpDelete("delete_category")]
        public async Task<ActionResult<DTOResposta>> ApagarCategoria(int id)
        {
            var resposta = await categoria.DeletarCategoria(id);

            return Ok(resposta);
        }


    }
}