using SmartLeaf.Domain;

namespace SmartLeaf.Application.Interfaces
{
    public interface ITerrainRepository
    {
        Task<Terrain?> GetByGardenIdAsync(int gardenId);
        Task<Terrain?> GetByIdAsync(int id);
        Task<Terrain> CreateAsync(Terrain terrain);
        Task UpdateAsync(Terrain terrain);
        Task DeleteAsync(int id);
    }
}
