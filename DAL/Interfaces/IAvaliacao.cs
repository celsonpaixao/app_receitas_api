using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DTO;
using api_receita.Models;

namespace api_receita.DAL.Interfaces
{
    public interface IAvaliacao
    {
        Task<DTOResposta> CadastrarAvaliar(int id_receita,int id_user ,AvaliacaoModel avaliacao);
        Task<DTOResposta> AtualizarAvaliar(int id_avaliacao, AvaliacaoModel avaliacaoAtualizada);
        Task<DTOResposta> ApagarAvaliacao(int id_avaliacao);
        Task<DTOResposta> ListarAvaliacao();
    }
}