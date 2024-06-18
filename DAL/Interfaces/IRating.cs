using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DTO;
using api_receita.Models;

namespace api_receita.DAL.Interfaces
{
    public interface IRating
    {
        Task<DTOResponse> Create_Rating(int id_receita, int id_user, RatingModel avaliacao);
        Task<DTOResponse> Update_Rating(int id_avaliacao, RatingModel avaliacaoAtualizada);
        Task<DTOResponse> Delete_Rating(int id_avaliacao);
        Task<DTOResponse> List_Rating_By_Recipe(int id_recipe);
    }
}