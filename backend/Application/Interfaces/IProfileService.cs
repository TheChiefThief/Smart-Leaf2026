using SmartLeaf.Application.DTOs;
using SmartLeaf.Domain;

namespace SmartLeaf.Application.Interfaces
{
    public interface IProfileService
    {
        Task<Profile?> GetByUserIdAsync(string userId);
        Task UpdateAsync(string userId, UpdateProfileRequest request);
    }
}
