namespace SmartLeaf.Domain
{
    public class PostLike
    {
        public int PostId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime LikeDate { get; set; }
    }
}
