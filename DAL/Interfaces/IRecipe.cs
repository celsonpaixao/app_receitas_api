using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DAL.Database;
using api_receita.DTO;
using app_receitas_api.Models;

namespace app_receitas_api.DAL.Interfaces
{
    public interface IRecipe
    {

        Task<DTOResponse> Create_Recipe(RecipeModel receita);
        Task<DTOResponse> Update_Recipe(int id_receita, RecipeModel receita);
        Task<DTOResponse> List_Recipe();
        Task<DTOResponse> List_Recipe_By_ID(int id);

        Task<DTOResponse> Delete_Recipe(int id);
    }
}