using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReferMeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReferMeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUsersCosmosDbService _cosmosDbService;
        private IEnumerable<User> users;

        public UsersController(IUsersCosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpPost("create-user")]
        public async Task<ActionResult> CreateUser(User user)
        {
            user.user_id = Guid.NewGuid().ToString();
            await _cosmosDbService.AddUserAsync(user);
            return Ok("User Created");
        }

        [HttpGet("get-users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            users = await _cosmosDbService.GetUsers();
            return users.ToList();
        }

        [HttpGet("get-user/{id}")]
        public async Task<ActionResult<User>> GetUser(string user_id)
        {
            return await _cosmosDbService.GetUserAsync(user_id);
        }

        [HttpPost("update-user/{id}")]
        public async Task<ActionResult<User>> GetUser(string user_id, User user)
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
    }
}
