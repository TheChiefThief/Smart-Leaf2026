// Models/LoginRequest.cs
namespace SmartLeaf.Domain
{
    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        
    }
}