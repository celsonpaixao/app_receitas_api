using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_receita.Models;
using Microsoft.IdentityModel.Tokens;

namespace app_receitas_api.Settings
{
    public static class TokenServices
    {
        public static object GenericToken(UserModel user)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Config.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]{
                    new Claim("userid", user.Id.ToString()),
                    new Claim("useremial", user.Email.ToString()),
                    new Claim("username", user. First_Name.ToString() + "" + user.Last_Name.ToString()),
                    
                }),
                Expires = DateTime.UtcNow.AddDays(360),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new
            {
                token = tokenString
            };
        }
    }
}