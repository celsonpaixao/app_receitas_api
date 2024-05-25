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
        Task<DTOResposta> ListarTodosUsuarios();
    }
}