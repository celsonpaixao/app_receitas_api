using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DAL.Database;
using api_receita.DTO;
using app_receitas_api.DAL.Interfaces;
using app_receitas_api.Models;

namespace app_receitas_api.DAL.Repositorys
{
    public class ReceitaRepository : IReceitas
    {
        private AppReceitasDbContext dbContext;
        public ReceitaRepository(AppReceitasDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DTOResposta> ListarReceitas()
        {
            DTOResposta resposta = new DTOResposta();

            try
            {
                var response = from user in dbContext.Tb_User
                               join receita in dbContext.Tb_Receita on user.Id equals receita.UserId
                               select new
                               {
                                   Title = receita.Title,
                                   Description = receita.Description,
                                   Ingredients = receita.Ingredients,
                                   Materials = receita.Materials,
                                   Instructions = receita.Instructions,
                                   Admin = new
                                   {
                                       Name = user.Primeiro_Name + " " + user.Ultimo_Name,

                                   }


                               };

                resposta.resposta = response;
                resposta.mensagem = "Sucesso";
            }
            catch (System.Exception e)
            {

                resposta.mensagem = $"Alguma Coisa deu errado : {e.ToString()}";
            }

            return resposta;
        }

        public async Task<DTOResposta> PublicarReceitas(ReceitaModel receita)
        {
            DTOResposta resposta = new DTOResposta();

            try
            {
                dbContext.Tb_Receita.Add(receita);
                await dbContext.SaveChangesAsync();

                resposta.resposta = receita;
                resposta.mensagem = "Receita Publicada com sucesso !!";
            }
            catch (System.Exception e)
            {

                resposta.mensagem = $"Alguma coisa deu errado {e.ToString()}";
            }
            return resposta;
        }
    }
}