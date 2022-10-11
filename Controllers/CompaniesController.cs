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
    public class CompaniesController : Controller
    {
        private readonly ICompaniesCosmosDbService _cosmosDbService;
        private IEnumerable<Company> companies;

        public CompaniesController(ICompaniesCosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpPost("create-company")]
        public async Task<ActionResult> CreateCompany(Company company)
        {
            company.user_id = Guid.NewGuid().ToString();
            await _cosmosDbService.AddCompanyAsync(company);
            return Ok("Company Created");
        }

        [HttpGet("get-companies")]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            companies = await _cosmosDbService.GetCompanies();
            return companies.ToList();
        }

        [HttpGet("get-company/{id}")]
        public async Task<ActionResult<Company>> GetCompany(string company_id)
        {
            return await _cosmosDbService.GetCompanyAsync(company_id);
        }

        [HttpPost("update-company/{id}")]
        public async Task<ActionResult<Company>> GetCompany(string company_id, Company company)
        {
            await _cosmosDbService.UpdateCompanyAsync(company_id, company);
            return Ok("Company Updated Successfully");
        }

        [HttpPost("delete-company/{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(string company_id)
        {
            await _cosmosDbService.DeleteCompanyAsync(company_id);
            return Ok("Company Deleted Successfully");
        }
    }
}
