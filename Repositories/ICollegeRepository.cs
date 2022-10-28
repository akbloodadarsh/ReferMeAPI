using ReferMeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReferMeAPI.Repositories
{
    public interface ICollegeRepository
    {
        Task<string> CreateCollege(College college);
        Task<IEnumerable<College>> GetColleges();
        Task<College> GetCollege(string college_id);
        Task<string> UpdateCollege(string college_id, College college);
        Task<string> DeleteCollege(string college_id);
    }
}
