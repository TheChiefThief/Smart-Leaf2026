using SmartLeaf.Application.DTOs;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Domain;

namespace SmartLeaf.Services
{
    public class SocialService : ISocialService
    {
        private readonly ISocialRepository _repo;

        public SocialService(ISocialRepository repo) => _repo = repo;

        public async Task<IEnumerable<PostResponse>> GetFeedAsync(string userId, int page, int pageSize)
        {
            var posts = await _repo.GetFeedAsync(userId, page, pageSize);
            return posts.Select(Map);
        }

        public async Task<PostResponse> CreatePostAsync(string userId, CreatePostRequest request)
        {
            var post = new Post
            {
                UserId = userId,
                PlantId = request.PlantId,
                TopicId = request.TopicId,
                ImageUrl = request.ImageUrl,
                Description = request.Descripcion,
                PostDate = DateTime.UtcNow
            };
            var created = await _repo.CreatePostAsync(post);
            return Map(created);
        }

        public async Task LikePostAsync(int postId, string userId) =>
            await _repo.LikePostAsync(postId, userId);

        public async Task UnlikePostAsync(int postId, string userId) =>
            await _repo.UnlikePostAsync(postId, userId);

        public async Task AddCommentAsync(int postId, string userId, CommentRequest request)
        {
            var comment = new PostComment
            {
                PostId = postId,
                UserId = userId,
                Content = request.Content,
                CommentDate = DateTime.UtcNow
            };
            await _repo.AddCommentAsync(comment);
        }

        public async Task FollowUserAsync(string followerId, string followingId) =>
            await _repo.FollowUserAsync(followerId, followingId);

        public async Task UnfollowUserAsync(string followerId, string followingId) =>
            await _repo.UnfollowUserAsync(followerId, followingId);

        private static PostResponse Map(Post p) => new()
        {
            Id = p.Id,
            UserId = p.UserId,
            PlantId = p.PlantId,
            ImageUrl = p.ImageUrl,
            Descripcion = p.Description,
            PostDate = p.PostDate
        };
    }
}
