using Microsoft.Azure.Cosmos;
using ReferMeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using College = ReferMeAPI.Model.College;

namespace ReferMeAPI
{
    public interface ICollegesCosmosDbService
    {
        Task<IEnumerable<College>> GetColleges();
        Task<College> GetCollegeAsync(string id);
        Task AddCollegeAsync(College College);
        Task UpdateCollegeAsync(string id, College college);
        Task DeleteCollegeAsync(string id);
    }

    public class CollegesCosmosDbService : ICollegesCosmosDbService
    {
        private Container _container;

        public CollegesCosmosDbService(
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddCollegeAsync(College college)
        {
            await _container.CreateItemAsync(college, new PartitionKey(college.college_id));
        }

        public async Task DeleteCollegeAsync(string college_id)
        {
            await _container.DeleteItemAsync<College>(college_id, new PartitionKey(college_id));
        }

        public async Task<College> GetCollegeAsync(string college_id)
        {
            try
            {
                var response = await _container.ReadItemAsync<College>(college_id, new PartitionKey(college_id));
                return response.Resource;
            }
            catch (CosmosException) //For handling item not found and other exceptions
            {
                return null;
            }
        }

        public async Task<IEnumerable<College>> GetColleges()
        {
            var query = _container.GetItemQueryIterator<College>(new QueryDefinition("SELECT * FROM Colleges"));

            var results = new List<College>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateCollegeAsync(string college_id, College college)
        {
            await _container.UpsertItemAsync(college, new PartitionKey(college_id));
        }
    }
}
