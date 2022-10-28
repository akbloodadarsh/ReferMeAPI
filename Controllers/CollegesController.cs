using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReferMeAPI.Model;
using ReferMeAPI.Repositories;
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
        private ICollegeRepository _collegeRepository;

        public CollegesController(ICollegeRepository collegeRepository)
        {
            _collegeRepository = collegeRepository;
        }

        [HttpPost("create-college")]
        public async Task<ActionResult> CreateCollege(College college)
        {
            await _collegeRepository.CreateCollege(college);
            return Ok("College Created");
        }

        [HttpGet("get-colleges")]
        public async Task<IEnumerable<College>> GetColleges()
        {
            return await _collegeRepository.GetColleges();
        }

        [HttpGet("get-college/{id}")]
        public async Task<College> GetCollege(string college_id)
        {
            return await _collegeRepository.GetCollege(college_id);
        }

        [HttpPost("update-college/{id}")]
        public async Task<ActionResult> UpdateCollege(string college_id, College college)
        {
            await _collegeRepository.UpdateCollege(college_id, college);
            return Ok("College Updated Successfully");
        }

        [HttpPost("delete-college/{id}")]
        public async Task<ActionResult> DeleteCollege(string college_id)
        {
            await _collegeRepository.DeleteCollege(college_id);
            return Ok("College Deleted Successfully");
        }
    }
}
