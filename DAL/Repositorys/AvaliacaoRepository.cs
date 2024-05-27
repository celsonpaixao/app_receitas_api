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
    public class AvaliacaoRepository : IAvaliacao
    {
        private readonly AppReceitasDbContext dbContext;

        public AvaliacaoRepository(AppReceitasDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DTOResposta> ApagarAvaliacao(int id_avaliacao)
        {
            DTOResposta resposta = new DTOResposta();
            try
            {
                var avaliacao = await dbContext.Tb_Avaliacao.FindAsync(id_avaliacao);
                if (avaliacao == null)
                {
                    resposta.mensagem = "Avaliação não encontrada";
                    return resposta;
                }

                dbContext.Tb_Avaliacao.Remove(avaliacao);
                await dbContext.SaveChangesAsync();

                resposta.mensagem = "Avaliação apagada com sucesso";
            }
            catch (Exception e)
            {
                resposta.mensagem = $"Erro ao apagar avaliação: {e.Message}";
            }

            return resposta;
        }

        public async Task<DTOResposta> AtualizarAvaliar(int id_avaliacao, AvaliacaoModel avaliacaoAtualizada)
        {
            DTOResposta resposta = new DTOResposta();
            try
            {
                var avaliacao = await dbContext.Tb_Avaliacao.FindAsync(id_avaliacao);
                if (avaliacao == null)
                {
                    resposta.mensagem = "Avaliação não encontrada";
                    return resposta;
                }

                avaliacao.Value = avaliacaoAtualizada.Value;
                avaliacao.Message = avaliacaoAtualizada.Message;

                dbContext.Tb_Avaliacao.Update(avaliacao);
                await dbContext.SaveChangesAsync();

                resposta.resposta = avaliacao;
                resposta.mensagem = "Avaliação atualizada com sucesso";
            }
            catch (Exception e)
            {
                resposta.mensagem = $"Erro ao atualizar avaliação: {e.Message}";
            }

            return resposta;
        }

        public async Task<DTOResposta> CadastrarAvaliar(int id_receita, int id_user, AvaliacaoModel avaliacao)
        {
            DTOResposta resposta = new DTOResposta();
            try
            {
                // Verifica se a receita existe
                var receita = await dbContext.Tb_Receita.FindAsync(id_receita);

                if (receita == null)
                {
                    resposta.mensagem = "Receita não encontrada";
                    return resposta;
                }

                // Verifica se o usuário existe
                var user = await dbContext.Tb_User.FindAsync(id_user);
                if (user == null)
                {
                    resposta.mensagem = "Usuário não encontrado";
                    return resposta;
                }

                // Associa a avaliação ao usuário
                avaliacao.Id_User = id_user;

                // Adiciona a nova avaliação
                dbContext.Tb_Avaliacao.Add(avaliacao);
                await dbContext.SaveChangesAsync();

                // Cria a entrada na tabela associativa
                var receitaAvaliacao = new Receita_AvaliacaoModel
                {
                    Id_Avaliacao = avaliacao.Id, // Assume que a avaliação tem um Id gerado automaticamente
                    Id_Receita = id_receita
                };

                dbContext.Tb_Receita_Avaliacao.Add(receitaAvaliacao);
                await dbContext.SaveChangesAsync();

                resposta.resposta = avaliacao;
                resposta.mensagem = "Avaliação cadastrada com sucesso";
            }
            catch (Exception e)
            {
                resposta.mensagem = $"Erro ao cadastrar avaliação: {e.Message}";
            }

            return resposta;
        }



        public async Task<DTOResposta> ListarAvaliacao()
        {
            DTOResposta resposta = new DTOResposta();

            try
            {
                var response = await dbContext.Tb_Avaliacao
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

                resposta.resposta = response;
                resposta.mensagem = "Sucesso";
            }
            catch (Exception e)
            {
                resposta.mensagem = $"Alguma coisa deu errado: {e.Message}";
            }

            return resposta;
        }
    }
}
