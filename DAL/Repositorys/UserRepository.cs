using Microsoft.EntityFrameworkCore;
using api_receita.DAL.Database;
using api_receita.DAL.Interfaces;
using api_receita.DTO;
using api_receita.Models;
using app_receitas_api.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api_receita.DAL.Repositorys
{
    public class UserRepository : IUser
    {
        private readonly ReceitasDbContext dbContext;

        public UserRepository(ReceitasDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DTOResponse> Update_User(int id_user, UserModel userAtualizado)
        {
            DTOResponse response = new DTOResponse();
            try
            {
                var user = await dbContext.Tb_User.FindAsync(id_user);
                if (user == null)
                {
                    response.message = "Usuário não encontrado";
                    return response;
                }

                // Atualiza os campos do usuário
                user.Primeiro_Name = userAtualizado.Primeiro_Name;
                user.Ultimo_Name = userAtualizado.Ultimo_Name;
                user.Email = userAtualizado.Email;
                user.Password = userAtualizado.Password;

                dbContext.Tb_User.Update(user);
                await dbContext.SaveChangesAsync();

                response.response = user;
                response.message = "Usuário atualizado com sucesso";
            }
            catch (Exception e)
            {
                response.message = $"Erro ao atualizar usuário: {e.Message}";
            }

            return response;
        }


        public async Task<DTOResponse> Create_User(UserModel user)
        {
            DTOResponse response = new DTOResponse();

            try
            {
                // Verificar se o usuário já existe
                bool userExists = await dbContext.Tb_User
                    .AnyAsync(u => u.Email == user.Email);

                if (userExists)
                {
                    response.message = "Usuário já existe com este email!";
                    return response;
                }

                // Hashear a senha
                var passwordHasher = new PasswordHasher<UserModel>();
                user.Password = passwordHasher.HashPassword(user, user.Password);

                // Adicionar novo usuário
                dbContext.Tb_User.Add(user);
                await dbContext.SaveChangesAsync();

                response.response = user;
                response.message = "Usuário cadastrado com sucesso!";
            }
            catch (Exception e)
            {
                response.message = "Algo deu errado! " + e.Message;
            }

            return response;
        }

        public async Task<DTOResponse> Delete_User(int id_user)
        {
            DTOResponse response = new DTOResponse();
            try
            {
                var user = await dbContext.Tb_User.FindAsync(id_user);
                if (user == null)
                {
                    response.message = "User not find";
                    return response;
                }

                dbContext.Tb_User.Remove(user);
                await dbContext.SaveChangesAsync();

                response.message = "User deleted sucess";
            }
            catch (Exception e)
            {
                response.message = $" {e.Message}";
            }

            return response;
        }


        public async Task<DTOResponse> List_User()
        {
            DTOResponse response = new DTOResponse();

            try
            {
                var query = from user in dbContext.Tb_User
                          select new
                          {
                              user
                          };

                response.response = query;
                response.message = "Sucess";

            }
            catch (System.Exception e)
            {

                response.message = "Opps we have a problem " + e.Message;
                Console.WriteLine(e.Message);
            }

            return response;
        }

        public async Task<DTOResponse> Auth_User(string email, string password)
        {
            DTOResponse response = new DTOResponse();

            try
            {
                // Procurar o usuário pelo e-mail no banco de dados
                var user = await dbContext.Tb_User.FirstOrDefaultAsync(u => u.Email == email);

                // Verificar se todos os campos foram preenchidos
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    response.message = "You needs send all informations about you";
                    return response;
                }

                // Verificar se o usuário existe
                if (user == null)
                {
                    response.message = "This user not exists";
                    return response;
                }

                // Verificar se a senha está correta
                var passwordHasher = new PasswordHasher<UserModel>();
                var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);

                if (result == PasswordVerificationResult.Failed)
                {
                    response.message = "Password incorret";
                    return response;
                }

                // Gerar token JWT com base no usuário autenticado
                var token = TokenServices.GenericToken(user);

                // Autenticação bem-sucedida
                response.message = "User Auth sucess";
                response.response = token; // Retornar informações adicionais do usuário junto com o token
            }
            catch (Exception ex)
            {
                // Lidar com exceções, se necessário
                response.message = $"Opps we have a problem {ex.Message}";
            }

            return response;
        }


    }
}
