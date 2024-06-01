using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DAL.Database;
using api_receita.DAL.Interfaces;
using api_receita.DTO;
using api_receita.Models;
using Microsoft.EntityFrameworkCore;

namespace api_receita.DAL.Repositorys
{
    public class CategoryRepository : ICategory
    {
        private readonly ReceitasDbContext dbContext;

        public CategoryRepository(ReceitasDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DTOResponse> Upadate_Category(int id, CategoryModel categoria)
        {
            DTOResponse response = new DTOResponse();
            try
            {
                // Encontrar a categoria pelo id
                var _categoria = await dbContext.Tb_Categoria.FirstOrDefaultAsync(c => c.Id == id);
                if (_categoria == null)
                {
                    response.message = "Category not found!";
                    return response;
                }


                _categoria.Name = categoria.Name;

                // Atualizar a categoria no banco de dados
                dbContext.Tb_Categoria.Update(_categoria);
                await dbContext.SaveChangesAsync();

                response.response = _categoria;
                response.message = "Category updated successfully!";
            }
            catch (System.Exception e)
            {
                response.message = $"Opps we have a problem: {e.Message}";
            }

            return response;
        }


        public async Task<DTOResponse> Create_Category(CategoryModel categoria)
        {
            DTOResponse response = new DTOResponse();
            try
            {
                dbContext.Tb_Categoria.Add(categoria);
                await dbContext.SaveChangesAsync();

                response.response = categoria;
                response.message = "Sucess !";
            }
            catch (System.Exception e)
            {

                response.message = $"Opps we have a problem {e.Message}";
            }

            return response;
        }

        public async Task<DTOResponse> Delete_Category(int id)
        {
            DTOResponse response = new DTOResponse();
            try
            {
                var _categoria = await dbContext.Tb_Categoria.FirstOrDefaultAsync(c => c.Id == id);
                if (_categoria == null)
                {
                    response.message = "Category not found !";
                }
                dbContext.Tb_Categoria.Remove(_categoria);
                await dbContext.SaveChangesAsync();

                response.response = _categoria;
                response.message = "Category deletes sucess !";
            }
            catch (System.Exception e)
            {

                response.message = $"Opps we have a problem {e.Message}";
            }

            return response;
        }

        public async Task<DTOResponse> List_Category()
        {
            DTOResponse response = new DTOResponse();
            try
            {
                var query = from categoria in dbContext.Tb_Categoria
                               select new
                               {
                                   categoria
                               };

                response.response = query;
                response.message = "Sucess";
            }
            catch (System.Exception e)
            {

                response.message = $"Opps we have a problem {e.Message}";
            }
            return response;
        }
    }
}