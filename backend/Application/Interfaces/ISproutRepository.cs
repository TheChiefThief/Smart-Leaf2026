using SmartLeaf.Domain;

namespace SmartLeaf.Application.Interfaces
{
    public interface ISproutRepository
    {
        Task<IEnumerable<Sprout>> GetByTerrainIdAsync(int terrainId);
        Task<Sprout?> GetByIdAsync(int id);
        Task<Sprout> CreateAsync(Sprout sprout);
        Task UpdateAsync(Sprout sprout);
        Task DeleteAsync(int id);
    }
}
