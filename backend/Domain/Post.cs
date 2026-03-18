namespace SmartLeaf.Domain
{
    public class Post
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int? PlantId { get; set; }
        public int? TopicId { get; set; }
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime PostDate { get; set; }
    }
}
