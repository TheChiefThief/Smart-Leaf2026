namespace SmartLeaf.Domain
{
    public class Follow
    {
        public string FollowerId { get; set; } = string.Empty;
        public string FollowingId { get; set; } = string.Empty;
        public DateTime FollowDate { get; set; }
    }
}
