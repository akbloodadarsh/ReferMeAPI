using ReferMeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReferMeAPI.Repositories
{
    public class CollegeRepository : ICollegeRepository
    {
        private readonly ICollegesCosmosDbService _collegesCosmosDbService;

        public CollegeRepository(ICollegesCosmosDbService collegesCosmosDbService)
        {
            _collegesCosmosDbService = collegesCosmosDbService;
        }

        public async Task<string> CreateCollege(College college)
        {
            college.college_id = Guid.NewGuid().ToString();
            await _collegesCosmosDbService.AddCollegeAsync(college);
            return "College Created";
        }

        public async Task<IEnumerable<College>> GetColleges()
        {
            return await _collegesCosmosDbService.GetColleges();
        }

        public async Task<College> GetCollege(string college_id)
        {
            return await _collegesCosmosDbService.GetCollegeAsync(college_id);
        }

        public async Task<string> UpdateCollege(string college_id, College college)
        {
            await _collegesCosmosDbService.UpdateCollegeAsync(college_id, college);
            return "College Updated Successfully";
        }

        public async Task<string> DeleteCollege(string college_id)
        {
            await _collegesCosmosDbService.DeleteCollegeAsync(college_id);
            return "College Deleted Successfully";
        }
    }
}
