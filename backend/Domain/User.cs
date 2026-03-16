// Models/User.cs
namespace SmartLeaf.Domain
{
    public class User
    {
        public required string Email { get; set; }
        public required string Password { get; set; }  // En producción usar hash
        public required string Role { get; set; }
    }
}