using Microsoft.EntityFrameworkCore;
using api_receita.DAL.Database;
using api_receita.DAL.Interfaces;
using api_receita.DTO;
using api_receita.Models;
using app_receitas_api.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace api_receita.DAL.Repositorys
{
    public class UserRepository : IUser
    {
        private readonly ReceitasDbContext dbContext;


        public UserRepository(ReceitasDbContext _dbContext)
        {
            dbContext = _dbContext;

        }
        public async Task<DTOResponse> Create_User(string firstName, string lastName, string email, string password, string confirmPassword)
        {
            DTOResponse response = new DTOResponse();

            try
            {
                UserModel user = new UserModel
                {
                    First_Name = firstName,
                    Last_Name = lastName,
                    Email = email,
                    Password = password
                };

                // Validate if the email is valid
                var emailAttribute = new EmailAddressAttribute();
                if (!emailAttribute.IsValid(user.Email))
                {
                    response.message = "Invalid email format!";
                    response.statusCode = 401;
                    return response;
                }

                // Check if the user already exists
                bool userExists = await dbContext.Tb_User
                    .AnyAsync(u => u.Email == user.Email);

                if (userExists)
                {
                    response.message = "User already exists with this email!";
                    response.statusCode = 404;
                    return response;
                }

                // Check if the password and confirmation password match
                if (user.Password != confirmPassword)
                {
                    response.message = "Password and password confirmation do not match!";
                    response.statusCode = 401;
                    return response;
                }

                // Hash the password
                var passwordHasher = new PasswordHasher<UserModel>();
                user.Password = passwordHasher.HashPassword(user, user.Password);

          
                // Add the new user
                dbContext.Tb_User.Add(user);
                await dbContext.SaveChangesAsync();

                response.response = user;
                response.statusCode = 200;
                response.message = "User registered successfully!";
            }
            catch (Exception e)
            {
                response.statusCode = 500;
                response.message = "Oops, we have a problem! " + e.Message;
            }

            return response;
        }


        public async Task<DTOResponse> Update_User(int id_user, UserModel userAtualizado, IFormFile? newImage, string confirmpassword)
        {
            DTOResponse response = new DTOResponse();
            try
            {
                var user = await dbContext.Tb_User.FindAsync(id_user);
                if (user == null)
                {
                    response.message = "User not found";
                    return response;
                }

                // Validar se o email é válido
                var emailAttribute = new EmailAddressAttribute();
                if (!emailAttribute.IsValid(userAtualizado.Email))
                {
                    response.message = "Invalid email format!";
                    return response;
                }

                // Verificar se a senha e a confirmação da senha correspondem
                if (!string.IsNullOrEmpty(userAtualizado.Password) && userAtualizado.Password != confirmpassword)
                {
                    response.message = "Password and password confirmation do not match!";
                    return response;
                }

                // Atualiza os campos do usuário
                user.First_Name = userAtualizado.First_Name;
                user.Last_Name = userAtualizado.Last_Name;
                user.Email = userAtualizado.Email;

                // Verifica se uma nova senha foi fornecida e a criptografa
                if (!string.IsNullOrEmpty(userAtualizado.Password))
                {
                    var passwordHasher = new PasswordHasher<UserModel>();
                    user.Password = passwordHasher.HashPassword(user, userAtualizado.Password);
                }

                // Se uma nova imagem for fornecida
                if (newImage != null && newImage.Length > 0)
                {
                    // Se o usuário já tiver uma imagem, exclua-a
                    if (!string.IsNullOrEmpty(user.ImageURL))
                    {
                        var imagePath = Path.Combine("Uploads", user.ImageURL);
                        if (File.Exists(imagePath))
                        {
                            File.Delete(imagePath);
                        }
                    }

                    // Salva a nova imagem
                    var uploadsFolder = Path.Combine("wwwroot", "uploads", "users");
                    var uniqueFileName = DateTime.Now.Ticks + Path.GetExtension(newImage.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Criar o diretório se não existir
                    Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await newImage.CopyToAsync(fileStream);
                    }

                    // Atualiza o caminho da imagem do usuário
                    user.ImageURL = Path.Combine("uploads", "users", uniqueFileName).Replace("\\", "/");
                }

                dbContext.Tb_User.Update(user);
                await dbContext.SaveChangesAsync();

                // Generate a new token
                var token = TokenServices.GenericToken(user);

                response.response = new { user, token };
                response.message = "User updated successfully!";
                response.statusCode = 200;
            }
            catch (Exception e)
            {
                response.message = $"Oops, we have a problem: {e.Message}";
                response.statusCode = 500;
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
                    response.message = "User not found";
                    return response;
                }

                // Verificar se o usuário tem uma imagem associada
                if (!string.IsNullOrEmpty(user.ImageURL))
                {
                    // Excluir a imagem do sistema de arquivos
                    var imagePath = Path.Combine("Uploads", user.ImageURL);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                dbContext.Tb_User.Remove(user);
                await dbContext.SaveChangesAsync();

                response.message = "User deleted successfully";
            }
            catch (Exception e)
            {
                response.message = $"Opps we have a problem {e.Message}";
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
                // Validar se o email é válido
                var emailAttribute = new EmailAddressAttribute();
                if (!emailAttribute.IsValid(email))
                {
                    response.message = "Invalid email format!";
                    return response;
                }

                // Procurar o usuário pelo e-mail no banco de dados
                var user = await dbContext.Tb_User.FirstOrDefaultAsync(u => u.Email == email);

                // Verificar se o usuário existe
                if (user == null)
                {
                    response.message = "This user does not exist";
                    response.statusCode = 404; // Usuário não encontrado
                    return response;
                }

                // Verificar se a senha está correta
                var passwordHasher = new PasswordHasher<UserModel>();
                var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);

                if (result == PasswordVerificationResult.Failed)
                {
                    response.message = "Incorrect password";
                    response.statusCode = 401; // Senha incorreta
                    return response;
                }

                // Gerar token JWT com base no usuário autenticado
                var token = TokenServices.GenericToken(user);

                // Autenticação bem-sucedida
                response.message = "User Auth success";
                response.response = token; // Retornar informações adicionais do usuário junto com o token
                response.statusCode = 200; // OK
            }
            catch (Exception e)
            {
                // Lidar com exceções, se necessário
                response.message = $"Opps we have a problem {e.Message}";
                response.statusCode = 500; // Erro interno do servidor
            }

            return response;
        }



    }
}
