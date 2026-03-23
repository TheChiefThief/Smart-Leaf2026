using SmartLeaf.Application.DTOs;

namespace SmartLeaf.Application.Interfaces
{
    public interface IGardenService
    {
        Task<IEnumerable<GardenResponse>> GetUserGardensAsync(string userId);
        Task<GardenResponse> CreateGardenAsync(string userId, CreateGardenRequest request);
        Task UpdateGardenAsync(string userId, int id, UpdateGardenRequest request);
        Task<GardenResponse?> GetGardenByIdAsync(int id);
        Task DeleteGardenAsync(int id);
    }
}
