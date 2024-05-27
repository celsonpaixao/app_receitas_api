using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DAL.Database;
using api_receita.DTO;
using app_receitas_api.Models;

namespace app_receitas_api.DAL.Interfaces
{
    public interface IReceitas
    {

        Task<DTOResposta> PublicarReceitas(ReceitaModel receita);
        Task<DTOResposta> AtualizarReceitas(int id_receita, ReceitaModel receita);
        Task<DTOResposta> ListarReceitas();
        Task<DTOResposta> ListarPorID(int id);

        Task<DTOResposta> PagarReceita(int id);
    }
}