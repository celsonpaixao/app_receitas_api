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

        public async Task<ActionResult<DTOResponse>> CadastrarUsuario(string firstName, string lastName, string email, string password, string confirmPassword)
        {
        
            var resposta = await userServices.Create_User(firstName,lastName,email,password,confirmPassword);
            if (resposta == null || resposta.statusCode != 200)
            {
                // Se houve um erro, retorna um BadRequest com a mensagem de erro
                return BadRequest(resposta?.message);
            }
            
            return Ok(resposta);
        }
        // [Authorize]
        [HttpPost("auth_user")]
        public async Task<ActionResult<DTOResponse>> LogarUsuario(string email, string password)
        {
            // Chama o m�todo Auth_User do servi�o de usu�rio
            var resposta = await userServices.Auth_User(email, password);

            // Verifica se houve algum erro durante a autentica��o
            if (resposta == null || resposta.statusCode != 200)
            {
                // Se houve um erro, retorna um BadRequest com a mensagem de erro
                return BadRequest(resposta?.message);
            }

            // Se a autentica��o foi bem-sucedida, retorna um Ok com o DTO de resposta
            return Ok(resposta);
        }


        [HttpGet("list_all_user")]

        public async Task<ActionResult<DTOResponse>> ListarTodosUsers()
        {
            return Ok(await userServices.List_User());
        }
        [Authorize]
        [HttpPut("update_user")]
        public async Task<ActionResult<DTOResponse>> AtualizarUsuario([FromForm] UserModel user, IFormFile? image, int id, string confirmpassword)
        {
            var resposta = await userServices.Update_User(id, user, image, confirmpassword);

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