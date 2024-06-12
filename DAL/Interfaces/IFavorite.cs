using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DTO;
using api_receita.Models;

namespace api_receita.DAL.Interfaces
{
    public interface IFavorite
    {
        Task<DTOResponse> AddFavorite(int userId, int recipeId);
        Task<DTOResponse> RemoveFavorite(int userId, int recipeId);
        Task<DTOResponse> Liste_Favorite(int id_user);
    }
}