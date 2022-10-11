using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReferMeAPI.Services.JWTAuthentication
{
    public interface IJwtTokenManager
    {
        Task<string> Authenticate(string user_name, string password);
    }
    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly IUsersCosmosDbService _cosmosDbService;
        private readonly IConfiguration _configuration;

        public JwtTokenManager(IUsersCosmosDbService cosmosDbService, IConfiguration configuration)
        {
            _cosmosDbService = cosmosDbService;
            _configuration = configuration;
        }
        public async Task<string> Authenticate(string user_name, string password)
        {
            string response = await _cosmosDbService.AuthenticateUserAsync(user_name, password);

            if (response == "status:- failed, message:- user not exist" || response == "status:- failed, message:- password incorrect")
                return null;

            var key = _configuration.GetValue<string>("JwtConfig:Key");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user_name)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
