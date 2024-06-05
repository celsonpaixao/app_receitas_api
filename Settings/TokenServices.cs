using System;
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
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                      new Claim("userid", user.Id.ToString()),
                      new Claim("useremail", user.Email),
                      new Claim("firstname", user.First_Name),
                      new Claim("lastname", user.Last_Name),
                      new Claim("imageurl", user.ImageURL ?? string.Empty)
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
