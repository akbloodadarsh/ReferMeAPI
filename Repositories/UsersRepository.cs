using Microsoft.AspNetCore.Mvc;
using ReferMeAPI.Model;
using ReferMeAPI.Services.JWTAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReferMeAPI.Repositories
{
    public class UsersRepository : IUserRepository
    {

        private readonly IUsersCosmosDbService _usersCosmosDbService;
        private IEnumerable<User> users;
        private IJwtTokenManager _jwtTokenManager;
        
        public UsersRepository(IUsersCosmosDbService usersCosmosDbService, IJwtTokenManager jwtTokenManager)
        {
            _usersCosmosDbService = usersCosmosDbService;
            _jwtTokenManager = jwtTokenManager;
        }
        public async Task<ActionResult<string>> CreateUser(User user)
        {
            user.user_id = Guid.NewGuid().ToString();
            await _usersCosmosDbService.AddUserAsync(user);
            return "success";
        }

        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            users = await _usersCosmosDbService.GetUsers();
            return users.ToList();
        }

        public async Task<ActionResult<User>> GetUser(string user_id)
        {
            return await _usersCosmosDbService.GetUserAsync(user_id);
        }

        public async Task<ActionResult<string>> UpdateUser(string user_id, User user)
        {
            await _usersCosmosDbService.UpdateUserAsync(user_id, user);
            return "success";
        }

        public async Task<ActionResult<string>> DeleteUser(string user_id)
        {
            await _usersCosmosDbService.DeleteUserAsync(user_id);
            return "success";
        }
        
        public async Task<string> AuthenticateUser(string user_name, string password)
        {
            var token = await _jwtTokenManager.Authenticate(user_name, password);
            if (string.IsNullOrEmpty(token))
                return "unauthorized";
            return token;
        }

    }
}
