using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DTO;
using api_receita.Models;

namespace api_receita.DAL.Interfaces
{
    public interface ICategory
    {
        Task<DTOResponse> Create_Category(CategoryModel categoria);
        Task<DTOResponse> List_Category();
        Task<DTOResponse> Upadate_Category(int id, CategoryModel categoria);
        Task<DTOResponse> Delete_Category(int id);
    }
}