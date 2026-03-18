using SmartLeaf.Application.DTOs;

namespace SmartLeaf.Application.Interfaces
{
    public interface ISocialService
    {
        Task<IEnumerable<PostResponse>> GetFeedAsync(string userId, int page, int pageSize);
        Task<PostResponse> CreatePostAsync(string userId, CreatePostRequest request);
        Task LikePostAsync(int postId, string userId);
        Task UnlikePostAsync(int postId, string userId);
        Task AddCommentAsync(int postId, string userId, CommentRequest request);
        Task FollowUserAsync(string followerId, string followingId);
        Task UnfollowUserAsync(string followerId, string followingId);
    }
}
