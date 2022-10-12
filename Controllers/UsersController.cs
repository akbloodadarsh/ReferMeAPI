using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReferMeAPI.Model;
using ReferMeAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReferMeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private IEnumerable<User> users;
        private IUserRepository _userRepository;
        private IUsersCosmosDbService _usersCosmosDbService;

        public UsersController(IUserRepository userRepository, IUsersCosmosDbService usersCosmosDbService)
        {
            _userRepository = userRepository;
            _usersCosmosDbService = usersCosmosDbService;
        }
        /*
        [HttpPost("create-user")]
        public async Task<ActionResult> CreateUser(User user)
        {
            user.user_id = Guid.NewGuid().ToString();
            await _cosmosDbService.AddUserAsync(user);
            return Ok("User Created");
        }
        */

        [Authorize]
        [HttpGet("repo-get-users")]
        public async Task<ActionResult<IEnumerable<User>>> RepoGetUsers()
        {
            return await _userRepository.GetUsers();
        }

        [HttpGet("get-users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {

            var url = @"";
            //if (HttpContext.Request.Host.ToString() == "localhost")
            url = @"http://localhost:42553/api/Users/repo-get-users";

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(url);
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Environment.GetEnvironmentVariable("jwt-token"));
            var response = await httpClient.GetStringAsync(url);
            return Ok(response);
        }
        /*
        [HttpGet("get-user/{id}")]
        public async Task<ActionResult<User>> GetUser(string user_id)
        {
            return await _cosmosDbService.GetUserAsync(user_id);
        }

        [HttpPost("update-user/{id}")]
        public async Task<ActionResult<User>> UpdateUser(string user_id, User user)
        {
            await _cosmosDbService.UpdateUserAsync(user_id, user);
            return Ok("User Updated Successfully");
        }

        
        [HttpPost("delete-user/{id}")]
        public async Task<ActionResult<User>> DeleteUser(string user_id)
        {
            await _cosmosDbService.DeleteUserAsync(user_id);
            return Ok("User Deleted Successfully");
        }
        */

        [HttpPost("repo-authenticate-user")]
        public async Task<string> RepoAuthenticateUser(string user_name, string password)
        {
            var token = await _userRepository.AuthenticateUser(user_name, password);
            return token;
        }

        [AllowAnonymous]
        [HttpPost("authenticate-user")]
        public async Task<IActionResult> AuthenticateUser([FromBody]User user)
        {
            var token = await RepoAuthenticateUser(user.user_name, user.password);
            if (string.IsNullOrEmpty(token))
                return Unauthorized();
            Environment.SetEnvironmentVariable("jwt-token", token);
            return Ok(token);
        }
    }
}
