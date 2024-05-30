using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DTO;
using api_receita.Models;

namespace api_receita.DAL.Interfaces
{
    public interface IAvaluation
    {
        Task<DTOResponse> Create_Avaluation(int id_receita,int id_user ,AvaluationModel avaliacao);
        Task<DTOResponse> Update_Avaluation(int id_avaliacao, AvaluationModel avaliacaoAtualizada);
        Task<DTOResponse> Delete_Avaluation(int id_avaliacao);
        Task<DTOResponse> List_Avaluation();
    }
}