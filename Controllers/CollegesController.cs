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
    public class CollegesController : Controller
    {
        private readonly ICollegesCosmosDbService _cosmosDbService;
        private IEnumerable<College> colleges;

        public CollegesController(ICollegesCosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpPost("create-college")]
        public async Task<ActionResult> CreateCollege(College college)
        {
            college.college_id = Guid.NewGuid().ToString();
            await _cosmosDbService.AddCollegeAsync(college);
            return Ok("College Created");
        }

        [HttpGet("get-colleges")]
        public async Task<ActionResult<IEnumerable<College>>> GetColleges()
        {
            colleges = await _cosmosDbService.GetColleges();
            return colleges.ToList();
        }

        [HttpGet("get-college/{id}")]
        public async Task<ActionResult<College>> GetCollege(string college_id)
        {
            return await _cosmosDbService.GetCollegeAsync(college_id);
        }

        [HttpPost("update-college/{id}")]
        public async Task<ActionResult<College>> GetCollege(string college_id, College college)
        {
            await _cosmosDbService.UpdateCollegeAsync(college_id, college);
            return Ok("College Updated Successfully");
        }

        [HttpPost("delete-college/{id}")]
        public async Task<ActionResult<College>> DeleteCollege(string college_id)
        {
            await _cosmosDbService.DeleteCollegeAsync(college_id);
            return Ok("College Deleted Successfully");
        }
    }
}
