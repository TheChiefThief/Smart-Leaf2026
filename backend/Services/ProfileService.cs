using SmartLeaf.Application.DTOs;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Domain;

namespace SmartLeaf.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _repo;

        public ProfileService(IProfileRepository repo) => _repo = repo;

        public async Task<Profile?> GetByUserIdAsync(string userId) =>
            await _repo.GetByUserIdAsync(userId);

        public async Task UpdateAsync(string userId, UpdateProfileRequest request)
        {
            var profile = await _repo.GetByUserIdAsync(userId);
            if (profile == null) return;

            if (request.FullName != null) profile.FullName = request.FullName;
            if (request.AvatarUrl != null) profile.AvatarUrl = request.AvatarUrl;
            if (request.Biography != null) profile.Biography = request.Biography;
            if (request.IsPrivate.HasValue) profile.IsPrivate = request.IsPrivate.Value;

            await _repo.UpdateAsync(profile);
        }
    }
}
