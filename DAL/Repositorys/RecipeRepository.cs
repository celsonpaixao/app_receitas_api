using api_receita.DAL.Database;
using api_receita.DTO;
using api_receita.Models;
using app_receitas_api.DAL.Interfaces;
using app_receitas_api.Models;
using Microsoft.EntityFrameworkCore;

namespace app_receitas_api.DAL.Repositorys
{
    public class RecipeRepository : IRecipe
    {
        private readonly ReceitasDbContext dbContext;
        public RecipeRepository(ReceitasDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public Task<DTOResponse> Update_Recipe(int id_receita, RecipeModel receita)
        {
            throw new NotImplementedException();
        }

        public async Task<DTOResponse> List_Recipe_By_ID(int id)
        {
            DTOResponse response = new DTOResponse();
            try
            {
                var query = await dbContext.Tb_Receita
                    .Where(receita => receita.Id == id)
                 .Select(receita => new
                 {
                     receita.Id,
                     receita.Title,
                     receita.Description,
                     receita.Ingredients,
                     receita.Materials,
                     receita.Instructions,
                     receita.ImageURL,
                     Admin = dbContext.Tb_User
                            .Where(user => user.Id == receita.UserId)
                            .Select(user => user.Primeiro_Name + " " + user.Ultimo_Name)
                            .FirstOrDefault(),
                     Categorias = dbContext.Tb_Receita_Categoria
                            .Where(rc => rc.Id_Receita == receita.Id)
                            .Join(dbContext.Tb_Categoria,
                                  rc => rc.Id_Categoria,
                                  categoria => categoria.Id,
                                  (rc, categoria) => categoria.Name)
                            .ToList(),
                     Avaliacoes = dbContext.Tb_Receita_Avaliacao
                            .Where(ra => ra.Id_Receita == receita.Id)
                            .Join(dbContext.Tb_Avaliacao,
                                  ra => ra.Id_Avaliacao,
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
                                              user.Primeiro_Name,
                                              user.Ultimo_Name,
                                              user.Email
                                          })
                                          .FirstOrDefault()
                                  })
                            .ToList()
                 })
                    .ToListAsync();


                response.response = query;
                response.message = "Sucesso !";
            }
            catch (System.Exception e)
            {

                response.message = $"Alguma coisa de errado {e.Message}";
            }
            return response;
        }

        public async Task<DTOResponse> List_Recipe()
        {
            DTOResponse response = new DTOResponse();

            try
            {
                var query = await dbContext.Tb_Receita
                    .Select(receita => new
                    {
                        receita.Id,
                        receita.Title,
                        receita.Description,
                        receita.Ingredients,
                        receita.Materials,
                        receita.Instructions,
                        receita.ImageURL,
                        Admin = dbContext.Tb_User
                            .Where(user => user.Id == receita.UserId)
                            .Select(user => user.Primeiro_Name + " " + user.Ultimo_Name)
                            .FirstOrDefault(),
                        Categorias = dbContext.Tb_Receita_Categoria
                            .Where(rc => rc.Id_Receita == receita.Id)
                            .Join(dbContext.Tb_Categoria,
                                  rc => rc.Id_Categoria,
                                  categoria => categoria.Id,
                                  (rc, categoria) => categoria.Name)
                            .ToList(),
                        Avaliacoes = dbContext.Tb_Receita_Avaliacao
                            .Where(ra => ra.Id_Receita == receita.Id)
                            .Join(dbContext.Tb_Avaliacao,
                                  ra => ra.Id_Avaliacao,
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
                                              user.Primeiro_Name,
                                              user.Ultimo_Name,
                                              user.Email
                                          })
                                          .FirstOrDefault()
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


        public async Task<DTOResponse> Delete_Recipe(int id)
        {
            DTOResponse response = new DTOResponse();
            try
            {
                var _receita = await dbContext.Tb_Receita.FirstOrDefaultAsync(r => r.Id == id);
                if (_receita == null)
                {
                    response.message = "Não encontramos nenhuma receita";
                    return response;
                }

                dbContext.Tb_Receita.Remove(_receita);
                await dbContext.SaveChangesAsync();

                response.response = _receita;
                response.message = "Receita apagada com sucesso !";

            }
            catch (System.Exception e)
            {

                response.message = $"Alguma coisa deu errado {e.Message}";
            }
            return response;
        }

        public async Task<DTOResponse> Create_Recipe(RecipeModel receita)
        {
            DTOResponse response = new DTOResponse();
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                dbContext.Tb_Receita.Add(receita);
                await dbContext.SaveChangesAsync();

                foreach (var categoriaId in receita.Categorias)
                {
                    var receitaCategoria = new Recipe_CategoryModel
                    {
                        Id_Categoria = categoriaId,
                        Id_Receita = receita.Id
                    };
                    dbContext.Tb_Receita_Categoria.Add(receitaCategoria);
                }

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                response.response = receita;
                response.message = "Receita publicada com sucesso!";
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                response.message = $"Alguma coisa deu errado: {e.Message}";
            }

            return response;
        }

    }
}