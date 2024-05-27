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

        public async Task<DTOResposta> AtualizarUsuario(int id_user, UserModel userAtualizado)
        {
            DTOResposta resposta = new DTOResposta();
            try
            {
                var user = await dbContext.Tb_User.FindAsync(id_user);
                if (user == null)
                {
                    resposta.mensagem = "Usuário não encontrado";
                    return resposta;
                }

                // Atualiza os campos do usuário
                user.Primeiro_Name = userAtualizado.Primeiro_Name;
                user.Ultimo_Name = userAtualizado.Ultimo_Name;
                user.Email = userAtualizado.Email;
                user.Password = userAtualizado.Password;

                dbContext.Tb_User.Update(user);
                await dbContext.SaveChangesAsync();

                resposta.resposta = user;
                resposta.mensagem = "Usuário atualizado com sucesso";
            }
            catch (Exception e)
            {
                resposta.mensagem = $"Erro ao atualizar usuário: {e.Message}";
            }

            return resposta;
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
                resposta.mensagem = "Algo deu errado! " + e.Message;
                
            }

            return resposta;
        }

        public async Task<DTOResposta> DeletarUsuario(int id_user)
        {
            DTOResposta resposta = new DTOResposta();
            try
            {
                var user = await dbContext.Tb_User.FindAsync(id_user);
                if (user == null)
                {
                    resposta.mensagem = "Usuário não encontrado";
                    return resposta;
                }

                dbContext.Tb_User.Remove(user);
                await dbContext.SaveChangesAsync();

                resposta.mensagem = "Usuário deletado com sucesso";
            }
            catch (Exception e)
            {
                resposta.mensagem = $"Erro ao deletar usuário: {e.Message}";
            }

            return resposta;
        }


        public async Task<DTOResposta> ListarTodosUsuarios()
        {
            DTOResposta resposta = new DTOResposta();

            try
            {
                var req =  from user in dbContext.Tb_User
                          select new
                          {
                              user
                          };

                resposta.resposta =  req;
                resposta.mensagem = "Sucesso";

            }
            catch (System.Exception e)
            {

                resposta.mensagem = "Algo deu errado! " + e.Message;
                Console.WriteLine(e.Message);
            }

            return resposta;
        }

        public async Task<DTOResposta> LogarUsuario(string email, string password)
        {
            DTOResposta resposta = new DTOResposta();

            try
            {
                // Procurar o usuário pelo e-mail e senha no banco de dados
                var user = await dbContext.Tb_User.FirstOrDefaultAsync(u => u.Email == email);

                // Verificar se todos os campos foram preenchidos
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    resposta.mensagem = "Todos os campos são obrigatórios.";
                    return resposta;
                }

                // Verificar se o usuário existe
                if (user == null)
                {
                    resposta.mensagem = "Este usuário não existe.";
                    return resposta;
                }

                // Verificar se a senha está correta
                if (user.Password != password)
                {
                    resposta.mensagem = "A senha está incorreta.";
                    return resposta;
                }

                // Gerar token JWT com base no usuário autenticado
                var token = TokenServices.GenericToken(user);

                // Autenticação bem-sucedida
                resposta.mensagem = "Usuário autenticado com sucesso.";
                resposta.resposta = new { token }; // Retornar informações adicionais do usuário junto com o token
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
