using api_receita.DAL.Interfaces;
using api_receita.DTO;
using api_receita.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_receita.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUser userServices;
        public UserController(IUser _user)
        {
            userServices = _user;
        }
        [HttpPost("register_user")]

        public async Task<ActionResult<DTOResponse>> CadastrarUsuario([FromForm] UserModel user)
        {
            if (user.ImagePash != null)
            {
                var imgDirectory = Path.Combine("Uploads", "User_Avatar");
                var imgPath = Path.Combine(imgDirectory, user.ImagePash.FileName);

                // Ensure the directory exists
                Directory.CreateDirectory(imgDirectory);

                using (var fileStream = new FileStream(imgPath, FileMode.Create))
                {
                    await user.ImagePash.CopyToAsync(fileStream);
                }

                // Set the image path to the user object
                user.ImageURL = imgPath;
            }

            var resposta = userServices.Create_User(user);
            return Ok(await resposta);
        }
        // [Authorize]
        [HttpPost("auth_user")]

        public async Task<ActionResult<DTOResponse>> LogarUsuario(string email, string password)
        {
            var resposta = userServices.Auth_User(email, password);
            return Ok(await resposta);
        }

        [HttpGet("list_all_user")]

        public async Task<ActionResult<DTOResponse>> ListarTodosUsers()
        {
            return Ok(await userServices.List_User());
        }
        [Authorize]
        [HttpPut("update_user")]


        public async Task<ActionResult<DTOResponse>> AtualizarUsuario(int id_user, UserModel user)
        {
            var resposta = await userServices.Update_User(id_user, user);

            return Ok(resposta);
        }
        [Authorize]
        [HttpDelete("delete_user")]


        public async Task<ActionResult<DTOResponse>> DeletarUsuario(int id_user)
        {
            var resposta = await userServices.Delete_User(id_user);

            return Ok(resposta);
        }
    }
}