using SmartLeaf.Domain;

namespace SmartLeaf.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<Profile?> GetByIdAsync(string userId);
        Task<Profile?> GetByEmailAsync(string email);
        Task CreateProfileAsync(Profile profile);
    }
}
