using Microsoft.AspNetCore.Mvc;
using ReferMeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReferMeAPI.Repositories
{
    public interface IUserRepository
    {
        Task<ActionResult<string>> CreateUser(User user);
        Task<ActionResult<IEnumerable<User>>> GetUsers();
        Task<ActionResult<User>> GetUser(string user_id);
        Task<ActionResult<string>> UpdateUser(string user_id, User user);
        Task<ActionResult<string>> DeleteUser(string user_id);
        Task<string> AuthenticateUser(string user_name, string password);
    }
}
