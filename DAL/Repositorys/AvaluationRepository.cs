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
    public class AvaluationRepository : IAvaluation
    {
        private readonly ReceitasDbContext dbContext;

        public AvaluationRepository(ReceitasDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DTOResponse> Delete_Avaluation(int id_avaliacao)
        {
            DTOResponse response = new DTOResponse();
            try
            {
                var avaliacao = await dbContext.Tb_Avaliacao.FindAsync(id_avaliacao);
                if (avaliacao == null)
                {
                    response.message = "Avaluation not find";
                    return response;
                }

                dbContext.Tb_Avaliacao.Remove(avaliacao);
                await dbContext.SaveChangesAsync();

                response.message = "Avaluation deleted sucess";
            }
            catch (Exception e)
            {
                response.message = $"Opps we have a problem {e.Message}";
            }

            return response;
        }

        public async Task<DTOResponse> Update_Avaluation(int id_avaliacao, AvaluationModel avaliacaoAtualizada)
        {
            DTOResponse response = new DTOResponse();
            try
            {
                var avaliacao = await dbContext.Tb_Avaliacao.FindAsync(id_avaliacao);
                if (avaliacao == null)
                {
                    response.message = "Avaluation not find";
                    return response;
                }

                avaliacao.Value = avaliacaoAtualizada.Value;
                avaliacao.Message = avaliacaoAtualizada.Message;

                dbContext.Tb_Avaliacao.Update(avaliacao);
                await dbContext.SaveChangesAsync();

                response.response = avaliacao;
                response.message = "Avaluation updated sucess";
            }
            catch (Exception e)
            {
                response.message = $"Opps we have a problem {e.Message}";
            }

            return response;
        }

        public async Task<DTOResponse> Create_Avaluation(int id_receita, int id_user, AvaluationModel avaliacao)
        {
            DTOResponse response = new DTOResponse();
            try
            {
                // Verifica se a receita existe
                var receita = await dbContext.Tb_Receita.FindAsync(id_receita);

                if (receita == null)
                {
                    response.message = "Recipe not find";
                    return response;
                }

                // Verifica se o usuário existe
                var user = await dbContext.Tb_User.FindAsync(id_user);
                if (user == null)
                {
                    response.message = "User not find";
                    return response;
                }

                // Associa a avaliação ao usuário
                avaliacao.Id_User = id_user;

                // Adiciona a nova avaliação
                dbContext.Tb_Avaliacao.Add(avaliacao);
                await dbContext.SaveChangesAsync();

                // Cria a entrada na tabela associativa
                var receitaAvaliacao = new Recipe_AvaluationModel
                {
                    Id_Avaliacao = avaliacao.Id, // Assume que a avaliação tem um Id gerado automaticamente
                    Id_Receita = id_receita
                };

                dbContext.Tb_Receita_Avaliacao.Add(receitaAvaliacao);
                await dbContext.SaveChangesAsync();

                response.response = avaliacao;
                response.message = "Create Avaluation sucess";
            }
            catch (Exception e)
            {
                response.message = $"Opps we have a problem {e.Message}";
            }

            return response;
        }



        public async Task<DTOResponse> List_Avaluation()
        {
            DTOResponse response = new DTOResponse();

            try
            {
                var query = await dbContext.Tb_Avaliacao
                    .Select(avaliacao => new
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
                    .ToListAsync();

                response.response = query;
                response.message = "Sucess";
            }
            catch (Exception e)
            {
                response.message = $"Opps we have a problem {e.Message}";
            }

            return response;
        }
    }
}
