using ReferMeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReferMeAPI.Repositories
{
    public class CompaniesRepository : ICompaniesRepository
    {
        private readonly ICompaniesCosmosDbService _companiesCosmosDbService;

        public CompaniesRepository(ICompaniesCosmosDbService companiesCosmosDbService)
        {
            _companiesCosmosDbService = companiesCosmosDbService;
        }

        public async Task<string> CreateCompany(Company company)
        {
            company.user_id = Guid.NewGuid().ToString();
            await _companiesCosmosDbService.AddCompanyAsync(company);
            return "Company Created";
        }

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            return await _companiesCosmosDbService.GetCompanies();
        }

        public async Task<Company> GetCompany(string company_id)
        {
            return await _companiesCosmosDbService.GetCompanyAsync(company_id);
        }

        public async Task<string> UpdateCompany(string company_id, Company company)
        {
            await _companiesCosmosDbService.UpdateCompanyAsync(company_id, company);
            return "Company Updated Successfully";
        }

        public async Task<string> DeleteCompany(string company_id)
        {
            await _companiesCosmosDbService.DeleteCompanyAsync(company_id);
            return "Company Deleted Successfully";
        }
    }
}
