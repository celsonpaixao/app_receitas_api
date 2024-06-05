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
    public class FavoriteController : ControllerBase
    {
        private readonly IFavorite favorite;
        public FavoriteController(IFavorite _favorite)
        {
            favorite = _favorite;
        }
        
        [Authorize]
        [HttpGet("list_favorite")]
        public async Task<ActionResult<DTOResponse>> List_Favorite(int id_user)
        {
            var response = await favorite.Liste_Favorite(id_user);

            return Ok(response);
        }

        [Authorize]
        [HttpPost("add_favorite")]
        public async Task<ActionResult<DTOResponse>> Add_Favorite(FavoritesModel favorited)
        {
            var response = await favorite.Add_Favorite(favorited);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("remove_favorite")]
        public async Task<ActionResult<DTOResponse>> Remove_Favorite(int id)
        {
            var response = await favorite.Remove_Favorite(id);

            return Ok(response);
        }


    }
}