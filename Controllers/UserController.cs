using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<ActionResult<DTOResposta>> CadastrarUsuario([FromForm] UserModel user)
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

            var resposta = userServices.CadastarUsuario(user);
            return Ok(await resposta);
        }
        // [Authorize]
        [HttpPost("auth_user")]

        public async Task<ActionResult<DTOResposta>> LogarUsuario(string email, string password)
        {
            var resposta = userServices.LogarUsuario(email, password);
            return Ok(await resposta);
        }

        [HttpGet("list_all_user")]

        public async Task<ActionResult<DTOResposta>> ListarTodosUsers()
        {
            return Ok(await userServices.ListarTodosUsuarios());
        }
        [Authorize]
        [HttpPut("update_user")]
       

        public async Task<ActionResult<DTOResposta>> AtualizarUsuario(int id_user, UserModel user)
        {
            var resposta = await userServices.AtualizarUsuario(id_user, user);

            return Ok(resposta);
        }
        [Authorize]
        [HttpDelete("delete_user")]
       

        public async Task<ActionResult<DTOResposta>> DeletarUsuario(int id_user)
        {
            var resposta = await userServices.DeletarUsuario(id_user);

            return Ok(resposta);
        }
    }
}