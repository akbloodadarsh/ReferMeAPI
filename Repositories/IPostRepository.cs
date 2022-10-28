using Microsoft.AspNetCore.Mvc;
using ReferMeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReferMeAPI.Repositories
{
    public interface IPostRepository
    {
        Task<string> CreatePost(Post post);
        Task<IEnumerable<Post>> GetPosts();
        Task<IEnumerable<Post>> GetFilteredPosts(string experience, string company, string position, string job_location_country, string job_location_city, string role);
        Task<Post> GetPost(string post_id);
        Task<string> UpdatePost(string post_id, Post post);
        Task<string> DeletePost(string post_id);
    }
}
