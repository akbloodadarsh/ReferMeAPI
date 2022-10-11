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
    public class PostsController : Controller
    {
        private readonly IPostsCosmosDbService _cosmosDbService;
        private IEnumerable<Post> posts;

        public PostsController(IPostsCosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpPost("create-post")]
        public async Task<ActionResult> CreatePost(Post post)
        {
            post.post_id = Guid.NewGuid().ToString();
            await _cosmosDbService.AddPostAsync(post);
            return Ok("Post Created");
        }

        [HttpGet("get-posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            posts = await _cosmosDbService.GetPosts();
            return posts.ToList();
        }

        [HttpGet("get-post/{id}")]
        public async Task<ActionResult<Post>> GetPost(string post_id)
        {
            return await _cosmosDbService.GetPostAsync(post_id);
        }

        [HttpPost("update-post/{id}")]
        public async Task<ActionResult<Post>> GetPost(string post_id, Post post)
        {
            await _cosmosDbService.UpdatePostAsync(post_id, post);
            return Ok("Post Updated Successfully");
        }

        [HttpPost("delete-post/{id}")]
        public async Task<ActionResult<Post>> DeletePost(string post_id)
        {
            await _cosmosDbService.DeletePostAsync(post_id);
            return Ok("Post Deleted Successfully");
        }
    }
}
