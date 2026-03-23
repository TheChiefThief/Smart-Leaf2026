using SmartLeaf.Application.DTOs;

namespace SmartLeaf.Application.Interfaces
{
    public interface ITerrainService
    {
        Task<TerrainResponse?> GetByGardenIdAsync(int gardenId);
        Task<TerrainResponse> CreateAsync(CreateTerrainRequest request);
        Task UpdateAsync(int id, UpdateTerrainRequest request);
        Task DeleteAsync(int id);

        // Sprouts
        Task<SproutResponse> AddSproutAsync(CreateSproutRequest request);
        Task UpdateSproutAsync(int sproutId, UpdateSproutRequest request);
        Task DeleteSproutAsync(int sproutId);
    }
}
