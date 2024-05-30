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
        Task<DTOResponse> Create_User(UserModel user);
        Task<DTOResponse> Update_User(int id_user,UserModel user);
        Task<DTOResponse> Delete_User(int id_user);
        Task<DTOResponse> Auth_User(string email, string password);
        Task<DTOResponse> List_User();

       
    }
}