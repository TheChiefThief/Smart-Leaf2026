using SmartLeaf.Domain;

namespace SmartLeaf.Application.Interfaces
{
    public interface IPlantRepository
    {
        Task<IEnumerable<Plant>> GetByGardenIdAsync(int gardenId);
        Task<Plant?> GetByIdAsync(int id);
        Task<Plant> CreateAsync(Plant plant);
        Task UpdateStatusAsync(int id, string status);
        Task DeleteAsync(int id);
    }
}
