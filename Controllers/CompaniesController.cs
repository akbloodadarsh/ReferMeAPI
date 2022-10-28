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
    public class CompaniesController : Controller
    {
        private readonly ICompaniesRepository _companiesRepository;
        private IEnumerable<Company> companies;

        public CompaniesController(ICompaniesRepository companiesRepository)
        {
            _companiesRepository = companiesRepository;
        }

        [HttpPost("create-company")]
        public async Task<ActionResult> CreateCompany(Company company)
        {
            company.user_id = Guid.NewGuid().ToString();
            await _companiesRepository.CreateCompany(company);
            return Ok("Company Created");
        }

        [HttpGet("get-companies")]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            companies = await _companiesRepository.GetCompanies();
            return companies.ToList();
        }

        [HttpGet("get-company/{id}")]
        public async Task<ActionResult<Company>> GetCompany(string company_id)
        {
            return await _companiesRepository.GetCompany(company_id);
        }

        [HttpPost("update-company/{id}")]
        public async Task<ActionResult<Company>> UpdateCompany(string company_id, Company company)
        {
            await _companiesRepository.UpdateCompany(company_id, company);
            return Ok("Company Updated Successfully");
        }

        [HttpPost("delete-company/{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(string company_id)
        {
            await _companiesRepository.DeleteCompany(company_id);
            return Ok("Company Deleted Successfully");
        }
    }
}
