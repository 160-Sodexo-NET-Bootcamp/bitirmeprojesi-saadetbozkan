using Microsoft.IdentityModel.Tokens;
using ProductCatalog.Data.DataModels.Concrete;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductCatalog.Core
{
    public class TokenHandler
    {
        private const double JWT_Expiry = 60;
        public static string GenerateToken(string key, string issuer, string audience, User user)
        {

            var claims = new[]
            {
                    new Claim("UserId",user.Id.ToString()),
                    new Claim("Email",user.Email),
                    new Claim("Name",user.Name)              
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.Now.AddMinutes(JWT_Expiry), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
