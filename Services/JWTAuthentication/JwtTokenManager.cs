using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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

            if (response == "user not exist" || response == "password incorrect")
                return null;

            string user_id = response;

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

            var create_token = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(create_token);

            return JsonConvert.SerializeObject(new { token, user_id });
        }
    }
}
