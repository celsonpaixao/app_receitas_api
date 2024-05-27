using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DTO;
using api_receita.Models;

namespace api_receita.DAL.Interfaces
{
    public interface ICategoria
    {
        Task<DTOResposta> CadastrarCategoria(CategoriaModel categoria);
        Task<DTOResposta> ListaCategoria();
        Task<DTOResposta> AtualizarCategoria(int id, CategoriaModel categoria);
        Task<DTOResposta> DeletarCategoria(int id);
    }
}