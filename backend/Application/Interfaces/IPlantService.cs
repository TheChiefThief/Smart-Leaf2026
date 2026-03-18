using SmartLeaf.Application.DTOs;

namespace SmartLeaf.Application.Interfaces
{
    public interface IPlantService
    {
        Task<IEnumerable<PlantResponse>> GetByGardenIdAsync(int gardenId);
        Task<PlantResponse> AddPlantAsync(CreatePlantRequest request);
        Task UpdateStatusAsync(int id, string status);
        Task DeletePlantAsync(int id);
    }
}
