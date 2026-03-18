using SmartLeaf.Domain;

namespace SmartLeaf.Application.Interfaces
{
    public interface IProfileRepository
    {
        Task<Profile?> GetByUserIdAsync(string userId);
        Task<Profile> CreateAsync(Profile profile);
        Task UpdateAsync(Profile profile);
    }
}
