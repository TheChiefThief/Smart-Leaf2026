namespace SmartLeaf.Domain
{
    public class Profile
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public string? Biografia { get; set; }
        public bool EsPrivado { get; set; }
    }
}
