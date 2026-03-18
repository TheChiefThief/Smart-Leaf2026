using Npgsql;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Domain;

namespace SmartLeaf.Infrastructure.Repositories
{
    public class SocialRepository : ISocialRepository
    {
        private readonly string _connectionString;

        public SocialRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Supabase")!;
        }

        public async Task<IEnumerable<Post>> GetFeedAsync(string userId, int page, int pageSize)
        {
            var posts = new List<Post>();
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                @"SELECT p.id, p.user_id, p.plant_id, p.topic_id, p.image_url, p.description, p.post_date
                  FROM posts p
                  WHERE p.user_id = @uid::uuid
                     OR p.user_id IN (SELECT following_id FROM follows WHERE follower_id = @uid::uuid)
                  ORDER BY p.post_date DESC
                  LIMIT @limit OFFSET @offset",
                conn);
            cmd.Parameters.AddWithValue("uid", userId);
            cmd.Parameters.AddWithValue("limit", pageSize);
            cmd.Parameters.AddWithValue("offset", (page - 1) * pageSize);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                posts.Add(MapPost(reader));
            return posts;
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                @"INSERT INTO posts (user_id, plant_id, topic_id, image_url, description, post_date)
                  VALUES (@uid::uuid, @pid, @tid, @img, @desc, @date) RETURNING id",
                conn);
            cmd.Parameters.AddWithValue("uid", post.UserId);
            cmd.Parameters.AddWithValue("pid", post.PlantId.HasValue ? post.PlantId : DBNull.Value);
            cmd.Parameters.AddWithValue("tid", post.TopicId.HasValue ? post.TopicId : DBNull.Value);
            cmd.Parameters.AddWithValue("img", (object?)post.ImageUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("desc", post.Description);
            cmd.Parameters.AddWithValue("date", post.PostDate);
            post.Id = (int)(long)(await cmd.ExecuteScalarAsync())!;
            return post;
        }

        public async Task LikePostAsync(int postId, string userId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "INSERT INTO post_likes (post_id, user_id) VALUES (@pid, @uid::uuid) ON CONFLICT DO NOTHING",
                conn);
            cmd.Parameters.AddWithValue("pid", postId);
            cmd.Parameters.AddWithValue("uid", userId);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UnlikePostAsync(int postId, string userId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "DELETE FROM post_likes WHERE post_id = @pid AND user_id = @uid::uuid", conn);
            cmd.Parameters.AddWithValue("pid", postId);
            cmd.Parameters.AddWithValue("uid", userId);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<PostComment> AddCommentAsync(PostComment comment)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "INSERT INTO post_comments (post_id, user_id, content) VALUES (@pid, @uid::uuid, @content) RETURNING id",
                conn);
            cmd.Parameters.AddWithValue("pid", comment.PostId);
            cmd.Parameters.AddWithValue("uid", comment.UserId);
            cmd.Parameters.AddWithValue("content", comment.Content);
            comment.Id = (int)(long)(await cmd.ExecuteScalarAsync())!;
            return comment;
        }

        public async Task<IEnumerable<PostComment>> GetCommentsByPostIdAsync(int postId)
        {
            var comments = new List<PostComment>();
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, post_id, user_id, comment_date, content FROM post_comments WHERE post_id = @pid ORDER BY comment_date",
                conn);
            cmd.Parameters.AddWithValue("pid", postId);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                comments.Add(new PostComment
                {
                    Id          = (int)reader.GetInt64(0),
                    PostId      = (int)reader.GetInt64(1),
                    UserId      = reader.GetGuid(2).ToString(),
                    CommentDate = reader.GetDateTime(3),
                    Content     = reader.GetString(4)
                });
            return comments;
        }

        public async Task FollowUserAsync(string followerId, string followingId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "INSERT INTO follows (follower_id, following_id) VALUES (@frid::uuid, @fwid::uuid) ON CONFLICT DO NOTHING",
                conn);
            cmd.Parameters.AddWithValue("frid", followerId);
            cmd.Parameters.AddWithValue("fwid", followingId);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UnfollowUserAsync(string followerId, string followingId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "DELETE FROM follows WHERE follower_id = @frid::uuid AND following_id = @fwid::uuid", conn);
            cmd.Parameters.AddWithValue("frid", followerId);
            cmd.Parameters.AddWithValue("fwid", followingId);
            await cmd.ExecuteNonQueryAsync();
        }

        private static Post MapPost(NpgsqlDataReader r) => new()
        {
            Id          = (int)r.GetInt64(0),
            UserId      = r.GetGuid(1).ToString(),
            PlantId     = r.IsDBNull(2) ? null : (int)r.GetInt64(2),
            TopicId     = r.IsDBNull(3) ? null : (int)r.GetInt64(3),
            ImageUrl    = r.IsDBNull(4) ? null : r.GetString(4),
            Description = r.GetString(5),
            PostDate    = r.GetDateTime(6)
        };
    }
}
