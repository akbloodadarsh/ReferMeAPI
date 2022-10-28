using ReferMeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReferMeAPI.Repositories
{
    public interface ICompaniesRepository
    {
        Task<string> CreateCompany(Company company);
        Task<IEnumerable<Company>> GetCompanies();
        Task<Company> GetCompany(string company_id);
        Task<string> UpdateCompany(string company_id, Company company);
        Task<string> DeleteCompany(string company_id);
    }
}
