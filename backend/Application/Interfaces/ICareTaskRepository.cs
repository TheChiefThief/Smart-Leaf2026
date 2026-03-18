using SmartLeaf.Domain;

namespace SmartLeaf.Application.Interfaces
{
    public interface ICareTaskRepository
    {
        Task<IEnumerable<CareTask>> GetByPlantIdAsync(int plantId);
        Task<CareTask> CreateAsync(CareTask task);
        Task UpdateStatusAsync(int id, string status);
        Task DeleteAsync(int id);
    }
}
