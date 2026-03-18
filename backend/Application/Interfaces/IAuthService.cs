using SmartLeaf.Application.DTOs;

namespace SmartLeaf.Application.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Error)> RegisterAsync(RegisterRequest request);
        Task<string?> AuthenticateAsync(string email, string password);
    }
}
