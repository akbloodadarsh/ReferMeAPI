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
        Task<IEnumerable<Post>> GetFilteredPosts(string experience, string company, string position, string job_location_country, string job_location_city, string role);
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

        public async Task<IEnumerable<Post>> GetFilteredPosts(string experience_required, string for_company, string position, string job_location_country, string job_location_city, string role)
        {
            experience_required = experience_required == "" ? null : experience_required;
            for_company = for_company == "" ? null : for_company;
            position = position == "" ? null : position;
            job_location_country = job_location_country == "" ? null : job_location_country;
            job_location_city = job_location_city == "" ? null : job_location_city;
            role = role == "" ? null : role;

            string q = "SELECT * FROM Posts";

            if (experience_required!= null || for_company != null || position != null || job_location_country != null || job_location_city!=null || role!=null)
            {
                q += " AS u";
                if (experience_required != null)
                {
                    q += $" WHERE u.experience_required<={experience_required}";
                    experience_required = null;
                }
                else if (for_company != null)
                {
                    q += $" WHERE u.for_company='{for_company}'";
                    for_company = null;
                }
                else if (position != null)
                {
                    q += $" WHERE u.position='{position}'";
                    position = null;
                }
                else if (job_location_country != null)
                {
                    q += $" WHERE u.job_location_country='{job_location_country}'";
                    job_location_country = null;
                }
                else if (job_location_city != null)
                {
                    q += $" WHERE u.job_location_city='{job_location_city}'";
                    job_location_city = null;
                }
                else if (role != null)
                {
                    q += $" WHERE u.role='{role}'";
                    role = null;
                }
            }

            while (experience_required != null || for_company != null || position != null || job_location_country != null || job_location_city != null || role!=null)
            {
                if (experience_required != null)
                {
                    q += $" AND u.experience_required<={experience_required}";
                    experience_required = null;
                }
                else if (for_company != null)
                {
                    q += $" AND u.for_company='{for_company}'";
                    for_company = null;
                }
                else if (position != null)
                {
                    q += $" AND u.position='{position}'";
                    position = null;
                }
                else if (job_location_country != null)
                {
                    q += $" AND u.job_location_country='{job_location_country}'";
                    job_location_country = null;
                }
                else if (job_location_city != null)
                {
                    q += $" AND u.job_location_city='{job_location_city}'";
                    job_location_city = null;
                }
                else if (role != null)
                {
                    q += $" AND u.role='{role}'";
                    role = null;
                }
            }

            if(q[q.Length-1]==' ')
                q = q.Remove(q.Length - 1);
            
            var query = _container.GetItemQueryIterator<Post>(new QueryDefinition(q));
            
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
