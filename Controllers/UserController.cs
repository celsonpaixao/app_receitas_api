using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_receita.DAL.Interfaces;
using api_receita.DTO;
using api_receita.Models;
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
        [HttpPost("Cagastrar_User")]

        public async Task<ActionResult<DTOResposta>> CadastrarUsuario(UserModel user)
        {

            return Ok(await userServices.CadastarUsuario(user));
        }

        [HttpGet("Listar_Todos_Usuarios")]

        public async Task<ActionResult<DTOResposta>> ListarTodosUsers()
        {
            return Ok(await userServices.ListarTodosUsuarios());
        }
    }
}