using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DTO;
using api_receita.Models;

namespace api_receita.DAL.Interfaces
{
    public interface IUser
    {
        Task<DTOResposta> CadastarUsuario(UserModel user);
        Task<DTOResposta> AtualizarUsuario(int id_user,UserModel user);
        Task<DTOResposta> DeletarUsuario(int id_user);
        Task<DTOResposta> LogarUsuario(string email, string password);
        Task<DTOResposta> ListarTodosUsuarios();

       
    }
}