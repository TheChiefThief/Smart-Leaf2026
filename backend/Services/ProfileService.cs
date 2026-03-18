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

            if (request.NombreCompleto != null) profile.NombreCompleto = request.NombreCompleto;
            if (request.AvatarUrl != null) profile.AvatarUrl = request.AvatarUrl;
            if (request.Biografia != null) profile.Biografia = request.Biografia;
            if (request.EsPrivado.HasValue) profile.EsPrivado = request.EsPrivado.Value;

            await _repo.UpdateAsync(profile);
        }
    }
}
