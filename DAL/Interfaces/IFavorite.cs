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
        Task<DTOResponse> Add_Favorite(FavoritesModel favorited);
        Task<DTOResponse> Remove_Favorite(int id);
        Task<DTOResponse> Liste_Favorite(int id_user);
    }
}