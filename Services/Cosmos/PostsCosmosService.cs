using Microsoft.Azure.Cosmos;
using ReferMeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post = ReferMeAPI.Model.Post;

namespace ReferMeAPI
{
    public interface IPostsCosmosDbService
    {
        Task<IEnumerable<Post>> GetPosts();
        Task<Post> GetPostAsync(string post_id);
        Task AddPostAsync(Post post);
        Task UpdatePostAsync(string post_id, Post post);
        Task DeletePostAsync(string post_id);
    }

    public class PostsCosmosDbService : IPostsCosmosDbService
    {
        private Container _container;

        public PostsCosmosDbService(
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddPostAsync(Post post)
        {
            await _container.CreateItemAsync(post, new PartitionKey(post.post_id));
        }

        public async Task DeletePostAsync(string post_id)
        {
            await _container.DeleteItemAsync<Post>(post_id, new PartitionKey(post_id));
        }

        public async Task<Post> GetPostAsync(string post_id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Post>(post_id, new PartitionKey(post_id));
                return response.Resource;
            }
            catch (CosmosException) //For handling item not found and other exceptions
            {
                return null;
            }
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            var query = _container.GetItemQueryIterator<Post>(new QueryDefinition("SELECT * FROM Posts"));

            var results = new List<Post>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdatePostAsync(string post_id, Post post)
        {
            await _container.UpsertItemAsync(post, new PartitionKey(post_id));
        }
    }
}
