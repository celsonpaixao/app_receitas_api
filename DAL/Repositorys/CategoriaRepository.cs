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
    public class CategoriaRepository : ICategoria
    {
        private readonly AppReceitasDbContext dbContext;

        public CategoriaRepository(AppReceitasDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DTOResposta> AtualizarCategoria(int id, CategoriaModel categoria)
        {
            DTOResposta resposta = new DTOResposta();
            try
            {
                // Encontrar a categoria pelo id
                var _categoria = await dbContext.Tb_Categoria.FirstOrDefaultAsync(c => c.Id == id);
                if (_categoria == null)
                {
                    resposta.mensagem = "Categoria não encontrada!";
                    return resposta;
                }

                
                _categoria.Name = categoria.Name;

                // Atualizar a categoria no banco de dados
                dbContext.Tb_Categoria.Update(_categoria);
                await dbContext.SaveChangesAsync();

                resposta.resposta = _categoria;
                resposta.mensagem = "Categoria atualizada com sucesso!";
            }
            catch (System.Exception e)
            {
                resposta.mensagem = $"Alguma coisa deu errado: {e.Message}";
            }

            return resposta;
        }


        public async Task<DTOResposta> CadastrarCategoria(CategoriaModel categoria)
        {
            DTOResposta resposta = new DTOResposta();
            try
            {
                dbContext.Tb_Categoria.Add(categoria);
                await dbContext.SaveChangesAsync();

                resposta.resposta = categoria;
                resposta.mensagem = "Sucesso !";
            }
            catch (System.Exception e)
            {

                resposta.mensagem = $"Alguma coisa deu errado {e.Message}";
            }

            return resposta;
        }

        public async Task<DTOResposta> DeletarCategoria(int id)
        {
            DTOResposta resposta = new DTOResposta();
            try
            {
                var _categoria = await dbContext.Tb_Categoria.FirstOrDefaultAsync(c => c.Id == id);
                if (_categoria == null)
                {
                    resposta.mensagem = "Categoria não encontrada !";
                }
                dbContext.Tb_Categoria.Remove( _categoria);
                await dbContext.SaveChangesAsync();

                resposta.resposta = _categoria;
                resposta.mensagem = "Categoria apagada com sucesso !";
            }
            catch (System.Exception e)
            {

                resposta.mensagem = $"Alguma coisa deu errado {e.Message}";
            }

            return resposta;
        }

        public async Task<DTOResposta> ListaCategoria()
        {
            DTOResposta resposta = new DTOResposta();
            try
            {
                var response = from categoria in dbContext.Tb_Categoria
                               select new
                               {
                                   categoria
                               };

                resposta.resposta = response;
                resposta.mensagem = "Sucesso";
            }
            catch (System.Exception e)
            {

                resposta.mensagem = $"Alguma coisa de errado {e.Message}";
            }
            return resposta;
        }
    }
}