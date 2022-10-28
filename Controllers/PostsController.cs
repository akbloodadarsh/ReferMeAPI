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
    public class PostsController : Controller
    {
        private readonly IPostRepository _postRepository;
        private IEnumerable<Post> posts;

        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpPost("create-post")]
        public async Task<ActionResult> CreatePost(Post post)
        {
            post.post_id = Guid.NewGuid().ToString();
            await _postRepository.CreatePost(post);
            return Ok("Post Created");
        }

        [HttpGet("get-posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            posts = await _postRepository.GetPosts();
            return posts.ToList();
        }

        [HttpPost("get-filtered-posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetFilteredPosts(Post post)
        {
            string experience_required = post.experience_required.ToString(), for_company = post.for_company, position = post.position;
            string job_location_country = post.job_location_country, job_location_city = post.job_location_city, role = post.role;

            posts = await _postRepository.GetFilteredPosts(experience_required, for_company, position, job_location_country, job_location_city, role);
            return posts.ToList();
        }

        [HttpGet("get-post/{id}")]
        public async Task<ActionResult<Post>> GetPost(string post_id)
        {
            return await _postRepository.GetPost(post_id);
        }

        [HttpPost("update-post/{id}")]
        public async Task<ActionResult<Post>> UpdatePost(string post_id, Post post)
        {
            await _postRepository.UpdatePost(post_id, post);
            return Ok("Post Updated Successfully");
        }

        [HttpPost("delete-post/{id}")]
        public async Task<ActionResult<Post>> DeletePost(string post_id)
        {
            await _postRepository.DeletePost(post_id);
            return Ok("Post Deleted Successfully");
        }
    }
}
