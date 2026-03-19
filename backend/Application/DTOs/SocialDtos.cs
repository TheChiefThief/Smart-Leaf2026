namespace SmartLeaf.Application.DTOs
{
    public class CreatePostRequest
    {
        public int? PlantId { get; set; }
        public int? TopicId { get; set; }
        public string? ImageUrl { get; set; }
        public required string Descripcion { get; set; }
    }

    public class PostResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int? PlantId { get; set; }
        public string? ImageUrl { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public DateTime PostDate { get; set; }
    }

    public class CommentRequest
    {
        public required string Content { get; set; }
    }

    public class UpdateProfileRequest
    {
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Biography { get; set; }
        public bool? IsPrivate { get; set; }
    }
}
