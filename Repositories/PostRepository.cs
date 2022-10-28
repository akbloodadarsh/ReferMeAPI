using Microsoft.AspNetCore.Mvc;
using ReferMeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReferMeAPI.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IPostsCosmosDbService _postsCosmosDbService;

        public PostRepository(IPostsCosmosDbService postsCosmosDbService)
        {
            _postsCosmosDbService = postsCosmosDbService;
        }

        public async Task<string> CreatePost(Post post)
        {
            post.post_id = Guid.NewGuid().ToString();
            await _postsCosmosDbService.AddPostAsync(post);
            return "Post Created";
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return  await _postsCosmosDbService.GetPosts();
        }

        public async Task<IEnumerable<Post>> GetFilteredPosts(string experience_required, string for_company, string position, string job_location_country, string job_location_city, string role)
        {
            return await _postsCosmosDbService.GetFilteredPosts(experience_required, for_company, position, job_location_country, job_location_city, role);
        }

        public async Task<Post> GetPost(string post_id)
        {
            return await _postsCosmosDbService.GetPostAsync(post_id);
        }

        public async Task<string> UpdatePost(string post_id, Post post)
        {
            await _postsCosmosDbService.UpdatePostAsync(post_id, post);
            return "Post Updated Successfully";
        }

        public async Task<string> DeletePost(string post_id)
        {
            await _postsCosmosDbService.DeletePostAsync(post_id);
            return "Post Deleted Successfully";
        }
    }
}
