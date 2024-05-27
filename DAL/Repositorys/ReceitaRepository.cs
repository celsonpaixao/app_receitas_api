using api_receita.DAL.Database;
using api_receita.DTO;
using api_receita.Models;
using app_receitas_api.DAL.Interfaces;
using app_receitas_api.Models;
using Microsoft.EntityFrameworkCore;

namespace app_receitas_api.DAL.Repositorys
{
    public class ReceitaRepository : IReceitas
    {
        private readonly AppReceitasDbContext dbContext;
        public ReceitaRepository(AppReceitasDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public Task<DTOResposta> AtualizarReceitas(int id_receita, ReceitaModel receita)
        {
            throw new NotImplementedException();
        }

        public async Task<DTOResposta> ListarPorID(int id)
        {
            DTOResposta resposta = new DTOResposta();
            try
            {
                var response = await dbContext.Tb_Receita
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


                resposta.resposta = response;
                resposta.mensagem = "Sucesso !";
            }
            catch (System.Exception e)
            {

                resposta.mensagem = $"Alguma coisa de errado {e.Message}";
            }
            return resposta;
        }

        public async Task<DTOResposta> ListarReceitas()
        {
            DTOResposta resposta = new DTOResposta();

            try
            {
                var response = await dbContext.Tb_Receita
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

                resposta.resposta = response;
                resposta.mensagem = "Sucesso";
            }
            catch (Exception e)
            {
                resposta.mensagem = $"Alguma coisa deu errado: {e.Message}";
            }

            return resposta;
        }




        public async Task<DTOResposta> PagarReceita(int id)
        {
            DTOResposta resposta = new DTOResposta();
            try
            {
                var _receita = await dbContext.Tb_Receita.FirstOrDefaultAsync(r => r.Id == id);
                if (_receita == null)
                {
                    resposta.mensagem = "Não encontramos nenhuma receita";
                    return resposta;
                }

                dbContext.Tb_Receita.Remove(_receita);
                await dbContext.SaveChangesAsync();

                resposta.resposta = _receita;
                resposta.mensagem = "Receita apagada com sucesso !";

            }
            catch (System.Exception e)
            {

                resposta.mensagem = $"Alguma coisa deu errado {e.Message}";
            }
            return resposta;
        }

        public async Task<DTOResposta> PublicarReceitas(ReceitaModel receita)
        {
            DTOResposta resposta = new DTOResposta();
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                dbContext.Tb_Receita.Add(receita);
                await dbContext.SaveChangesAsync();

                foreach (var categoriaId in receita.Categorias)
                {
                    var receitaCategoria = new Receita_CategoriaModel
                    {
                        Id_Categoria = categoriaId,
                        Id_Receita = receita.Id
                    };
                    dbContext.Tb_Receita_Categoria.Add(receitaCategoria);
                }

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                resposta.resposta = receita;
                resposta.mensagem = "Receita publicada com sucesso!";
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                resposta.mensagem = $"Alguma coisa deu errado: {e.Message}";
            }

            return resposta;
        }

    }
}