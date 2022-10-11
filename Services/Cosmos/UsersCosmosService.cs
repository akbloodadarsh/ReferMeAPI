using Microsoft.Azure.Cosmos;
using ReferMeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User = ReferMeAPI.Model.User;

namespace ReferMeAPI
{
    public interface IUsersCosmosDbService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserAsync(string id);
        Task AddUserAsync(User item);
        Task UpdateUserAsync(string id, User item);
        Task DeleteUserAsync(string id);
        Task<string> AuthenticateUserAsync(string user_name, string password);
    }

    public class UsersCosmosDbService : IUsersCosmosDbService
    {
        private Container _container;

        public UsersCosmosDbService(
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddUserAsync(User item)
        {
            await _container.CreateItemAsync(item, new PartitionKey(item.user_id));
        }

        public async Task DeleteUserAsync(string id)
        {
            await _container.DeleteItemAsync<User>(id, new PartitionKey(id));
        }

        public async Task<User> GetUserAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<User>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException) //For handling item not found and other exceptions
            {
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var query = _container.GetItemQueryIterator<User>(new QueryDefinition("SELECT * FROM Users"));

            var results = new List<User>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateUserAsync(string id, User item)
        {
            await _container.UpsertItemAsync(item, new PartitionKey(id));
        }

        public async Task<string> AuthenticateUserAsync(string user_name, string password)
        {
            var query = _container.GetItemQueryIterator<User>(new QueryDefinition($"SELECT * FROM Users AS u WHERE u.user_name='{user_name}'"));

            var results = new List<User>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            if (results.Count() == 0)
                return "status:- failed, message:- user not exist";

            if (results[0].password == password)
                return "status:- success";

            return "status:- failed, message:- password incorrect";
        }
    }
}
