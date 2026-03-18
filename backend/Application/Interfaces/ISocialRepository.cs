using SmartLeaf.Domain;

namespace SmartLeaf.Application.Interfaces
{
    public interface ISocialRepository
    {
        Task<IEnumerable<Post>> GetFeedAsync(string userId, int page, int pageSize);
        Task<Post> CreatePostAsync(Post post);
        Task LikePostAsync(int postId, string userId);
        Task UnlikePostAsync(int postId, string userId);
        Task<PostComment> AddCommentAsync(PostComment comment);
        Task<IEnumerable<PostComment>> GetCommentsByPostIdAsync(int postId);
        Task FollowUserAsync(string followerId, string followingId);
        Task UnfollowUserAsync(string followerId, string followingId);
    }
}
