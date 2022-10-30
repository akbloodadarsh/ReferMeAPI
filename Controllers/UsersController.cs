using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReferMeAPI.Model;
using ReferMeAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ReferMeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private IUserRepository _userRepository;
        private IUsersCosmosDbService _usersCosmosDbService;

        public UsersController(IUserRepository userRepository, IUsersCosmosDbService usersCosmosDbService)
        {
            _userRepository = userRepository;
            _usersCosmosDbService = usersCosmosDbService;
        }

        [Authorize]
        [HttpPost("create-user")]
        public async Task<ActionResult> CreateUser(User user)
        {
            user.user_id = Guid.NewGuid().ToString();
            await _userRepository.CreateUser(user);
            return Ok("User Created");
        }

        [Authorize]
        [HttpGet("get-users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _userRepository.GetUsers();
        }

        [Authorize]
        [HttpGet("get-user/{id}")]
        public async Task<ActionResult<User>> GetUser(string user_id)
        {
            return await _userRepository.GetUser(user_id);
        }

        [Authorize]
        [HttpPost("update-user/{id}")]
        public async Task<ActionResult<User>> UpdateUser(string user_id, User user)
        {
            await _userRepository.UpdateUser(user_id, user);
            return Ok("User Updated Successfully");
        }

        [Authorize]
        [HttpPost("delete-user/{id}")]
        public async Task<ActionResult<User>> DeleteUser(string user_id)
        {
            await _userRepository.DeleteUser(user_id);
            return Ok("User Deleted Successfully");
        }

        [AllowAnonymous]
        [HttpPost("authenticate-user")]
        public async Task<string> AuthenticateUser([FromBody]User user)
        {
            var token = await _userRepository.AuthenticateUser(user.user_name, user.password);
            return JsonConvert.SerializeObject(new { token });
        }
    }
}
