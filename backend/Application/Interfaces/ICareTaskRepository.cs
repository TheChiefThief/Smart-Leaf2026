using SmartLeaf.Domain;

namespace SmartLeaf.Application.Interfaces
{
    public interface ICareTaskRepository
    {
        Task<IEnumerable<CareTask>> GetByPlantIdAsync(int plantId);
        Task<CareTask?> GetByIdAsync(int id);
        Task<CareTask> CreateAsync(CareTask task);
        Task UpdateStatusAsync(int id, string status);
        Task UpdateAsync(CareTask task);
        Task DeleteAsync(int id);
    }
}
