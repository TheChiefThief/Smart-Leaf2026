using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLeaf.Application.DTOs;
using SmartLeaf.Application.Interfaces;
using System.Security.Claims;

namespace SmartLeaf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SocialController : ControllerBase
    {
        private readonly ISocialService _service;

        public SocialController(ISocialService service) => _service = service;

        [Authorize]
        [HttpGet("feed")]
        public async Task<IActionResult> GetFeed([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var feed = await _service.GetFeedAsync(userId, page, pageSize);
            return Ok(feed);
        }

        [Authorize]
        [HttpPost("post")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var post = await _service.CreatePostAsync(userId, request);
            return Ok(post);
        }

        [Authorize]
        [HttpPost("like/{postId}")]
        public async Task<IActionResult> Like(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.LikePostAsync(postId, userId);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("like/{postId}")]
        public async Task<IActionResult> Unlike(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.UnlikePostAsync(postId, userId);
            return NoContent();
        }

        [Authorize]
        [HttpPost("comment/{postId}")]
        public async Task<IActionResult> Comment(int postId, [FromBody] CommentRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.AddCommentAsync(postId, userId, request);
            return NoContent();
        }

        [Authorize]
        [HttpPost("follow/{targetUserId}")]
        public async Task<IActionResult> Follow(string targetUserId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.FollowUserAsync(userId, targetUserId);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("follow/{targetUserId}")]
        public async Task<IActionResult> Unfollow(string targetUserId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.UnfollowUserAsync(userId, targetUserId);
            return NoContent();
        }
    }
}
