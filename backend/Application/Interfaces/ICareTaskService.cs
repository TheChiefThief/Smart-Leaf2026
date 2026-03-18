using SmartLeaf.Application.DTOs;

namespace SmartLeaf.Application.Interfaces
{
    public interface ICareTaskService
    {
        Task<IEnumerable<CareTaskResponse>> GetByPlantIdAsync(int plantId);
        Task<CareTaskResponse> CreateAsync(CreateCareTaskRequest request);
        Task UpdateStatusAsync(int id, string status);
        Task DeleteAsync(int id);
    }
}
