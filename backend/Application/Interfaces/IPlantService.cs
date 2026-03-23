using SmartLeaf.Application.DTOs;

namespace SmartLeaf.Application.Interfaces
{
    public interface IPlantService
    {
        Task<IEnumerable<PlantResponse>> GetByGardenIdAsync(int gardenId);
        Task<PlantResponse?> GetPlantByIdAsync(int id);
        Task<PlantResponse> AddPlantAsync(CreatePlantRequest request);
        Task UpdateStatusAsync(int id, string status);
        Task UpdatePlantAsync(int id, UpdatePlantRequest request);
        Task DeletePlantAsync(int id);
    }
}
