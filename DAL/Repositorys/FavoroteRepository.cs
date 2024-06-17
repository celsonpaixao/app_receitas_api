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
    public class FavoroteRepository : IFavorite
    {
        private readonly ReceitasDbContext dbContext;
        public FavoroteRepository(ReceitasDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DTOResponse> AddFavorite(int userId, int recipeId)
        {
            DTOResponse response = new DTOResponse();
            try
            {
                var existingFavorite = await dbContext.Tb_Favorite
                    .FirstOrDefaultAsync(favorite => favorite.User_id == userId && favorite.Recipe_id == recipeId);

                if (existingFavorite == null)
                {
                    FavoritesModel newFavorite = new FavoritesModel
                    {
                        User_id = userId,
                        Recipe_id = recipeId
                    };

                    dbContext.Tb_Favorite.Add(newFavorite);
                    await dbContext.SaveChangesAsync();
                    response.message = "Recipe favorited successfully!";
                }
                else
                {
                    response.message = "Recipe is already in favorites!";
                }
            }
            catch (Exception e)
            {
                response.message = "Oops, we have a problem: " + e.ToString();
            }
            return response;
        }
        public async Task<DTOResponse> Liste_Favorite(int id_user)
        {
            DTOResponse response = new DTOResponse();

            try
            {
                var query = await dbContext.Tb_Favorite
                    .Where(favorite => favorite.User_id == id_user)
                    .Select(favorite => new
                    {
                        favorite.Id,
                        Recipe = dbContext.Tb_Receita
                            .Where(recipe => recipe.Id == favorite.Recipe_id)
                            .Select(receita => new
                            {
                                receita.Id,
                                receita.Title,
                                receita.Description,
                                receita.Instructions,
                                receita.ImageURL,
                                Admin = dbContext.Tb_User
                                    .Where(user => user.Id == receita.UserId)
                                    .Select(user => user.First_Name + " " + user.Last_Name)
                                    .FirstOrDefault(),

                                // Buscar Categorias
                                Categorias = dbContext.Tb_RecipeCategory
                                    .Where(rc => rc.RecipeId == receita.Id)
                                    .Join(dbContext.Tb_Categoria,
                                        rc => rc.CategoryId,
                                        categoria => categoria.Id,
                                        (rc, categoria) => categoria.Name)
                                    .ToList(),

                                // Buscar Ingredientes
                                Ingredients = dbContext.Tb_Ingredients_Recipe
                                    .Where(ri => ri.RecipeId == receita.Id)
                                    .Join(dbContext.Tb_Ingredients,
                                        ri => ri.IngredientId,
                                        ingrediente => ingrediente.Id,
                                        (ri, ingrediente) => ingrediente.Name)
                                    .ToList(),

                                // Buscar Materiais
                                Materials = dbContext.Tb_Materials_Recipe
                                    .Where(rm => rm.RecipeId == receita.Id)
                                    .Join(dbContext.Tb_Materials,
                                        rm => rm.MaterialId,
                                        material => material.Id,
                                        (rm, material) => material.Name)
                                    .ToList(),

                                Avaliacoes = dbContext.Tb_Receita_Avaliacao
                                    .Where(ra => ra.RecipeId == receita.Id)
                                    .Join(dbContext.Tb_Avaliacao,
                                        ra => ra.RatingId,
                                        avaliacao => avaliacao.Id,
                                        (ra, avaliacao) => new
                                        {
                                            avaliacao.Id,
                                            avaliacao.Value,
                                            avaliacao.Message,
                                            User = dbContext.Tb_User
                                                .Where(user => user.Id == avaliacao.Id_User)
                                                .Select(user => new
                                                {
                                                    user.Id,
                                                    user.First_Name,
                                                    user.Last_Name,
                                                    user.Email
                                                })
                                                .FirstOrDefault()
                                        })
                                    .ToList()
                            })
                            .ToList()
                    })
                    .ToListAsync();

                response.response = query;
                response.message = "Sucesso";
            }
            catch (Exception e)
            {
                response.message = $"Alguma coisa deu errado: {e.Message}";
            }

            return response;
        }




        public async Task<DTOResponse> RemoveFavorite(int userId, int recipeId)
        {
            DTOResponse response = new DTOResponse();
            try
            {
                var existingFavorite = await dbContext.Tb_Favorite
                    .FirstOrDefaultAsync(favorite => favorite.User_id == userId && favorite.Recipe_id == recipeId);

                if (existingFavorite != null)
                {
                    dbContext.Tb_Favorite.Remove(existingFavorite);
                    await dbContext.SaveChangesAsync();
                    response.message = "Recipe removed from favorites!";
                }
                else
                {
                    response.message = "Recipe not found in favorites!";
                }
            }
            catch (Exception e)
            {
                response.message = "Oops, we have a problem: " + e.Message;
            }
            return response;
        }
    }
}