using Microsoft.Azure.Cosmos;
using ReferMeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company = ReferMeAPI.Model.Company;

namespace ReferMeAPI
{
    public interface ICompaniesCosmosDbService
    {
        Task<IEnumerable<Company>> GetCompanies();
        Task<Company> GetCompanyAsync(string company_id);
        Task AddCompanyAsync(Company company);
        Task UpdateCompanyAsync(string company_id, Company company);
        Task DeleteCompanyAsync(string company_id);
    }

    public class CompaniesCosmosDbService : ICompaniesCosmosDbService
    {
        private Container _container;

        public CompaniesCosmosDbService(
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddCompanyAsync(Company company)
        {
            await _container.CreateItemAsync(company, new PartitionKey(company.company_id));
        }

        public async Task DeleteCompanyAsync(string company_id)
        {
            await _container.DeleteItemAsync<Company>(company_id, new PartitionKey(company_id));
        }

        public async Task<Company> GetCompanyAsync(string company_id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Company>(company_id, new PartitionKey(company_id));
                return response.Resource;
            }
            catch (CosmosException) //For handling item not found and other exceptions
            {
                return null;
            }
        }

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            var query = _container.GetItemQueryIterator<Company>(new QueryDefinition("SELECT * FROM Companies"));

            var results = new List<Company>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateCompanyAsync(string company_id, Company company)
        {
            await _container.UpsertItemAsync(company, new PartitionKey(company_id));
        }
    }
}
