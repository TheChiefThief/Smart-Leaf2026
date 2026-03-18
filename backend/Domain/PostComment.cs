namespace SmartLeaf.Domain
{
    public class PostComment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime CommentDate { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
