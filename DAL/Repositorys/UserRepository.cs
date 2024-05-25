using Microsoft.EntityFrameworkCore;
using api_receita.DAL.Database;
using api_receita.DAL.Interfaces;
using api_receita.DTO;
using api_receita.Models;
using app_receitas_api.Settings;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api_receita.DAL.Repositorys
{
    public class UserRepository : IUser
    {
        private readonly AppReceitasDbContext dbContext;

        public UserRepository(AppReceitasDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DTOResposta> CadastarUsuario(UserModel user)
        {
            DTOResposta resposta = new DTOResposta();

            try
            {
                // Verificar se o usuário já existe
                bool userExists = await dbContext.Tb_User
                    .AnyAsync(u => u.Email == user.Email);

                if (userExists)
                {
                    resposta.mensagem = "Usuário já existe com este email!";
                    return resposta;
                }

                // Se não existe, adicionar novo usuário
                UserModel newUser = user;
                dbContext.Tb_User.Add(newUser);

                await dbContext.SaveChangesAsync();
                resposta.resposta = newUser;
                resposta.mensagem = "Usuário cadastrado com sucesso!";
            }
            catch (Exception e)
            {
                resposta.mensagem = "Algo deu errado! " + e.ToString();
                Console.WriteLine(e.Message);
            }

            return resposta;
        }

        public async Task<DTOResposta> ListarTodosUsuarios()
        {
            DTOResposta resposta = new DTOResposta();

            try
            {
                var req = from user in dbContext.Tb_User
                          select new
                          {
                              user
                          };

                resposta.resposta = req;
                resposta.mensagem = "Sucesso";

            }
            catch (System.Exception e)
            {

                resposta.mensagem = "Algo deu errado! " + e.ToString();
                Console.WriteLine(e.Message);
            }

            return resposta;
        }

        public async Task<DTOResposta> LogarUsuario(string email, string password)
        {
            DTOResposta resposta = new DTOResposta();


            var user = await dbContext.Tb_User.FirstOrDefaultAsync(u => u.Email == email);

            try
            {
                // Verificar se o usuário foi encontrado e se a senha está correta
                if (user != null && user.Password == password)
                {
                    // Gerar token JWT com base no usuário autenticado
                    var token = TokenServices.GenericToken(user);

                    // Autenticação bem-sucedida
                    resposta.mensagem = "Usuário autenticado com sucesso.";
                    resposta.resposta = new { user, token }; // Você pode retornar informações adicionais do usuário, se desejar
                }
                else
                {
                    // Usuário não encontrado ou senha incorreta
                    resposta.mensagem = "E-mail ou senha incorretos.";
                }
            }
            catch (System.Exception ex)
            {
                // Lidar com exceções, se necessário
                resposta.mensagem = $"Ocorreu um erro durante a autenticação: {ex.Message}";
            }

            return resposta;
        }


    }
}
