using SmartLeaf.Domain;

namespace SmartLeaf.Application.Interfaces
{
    public interface IGardenRepository
    {
        Task<IEnumerable<Garden>> GetByUserIdAsync(string userId);
        Task<Garden?> GetByIdAsync(int id);
        Task<Garden> CreateAsync(Garden garden);
        Task DeleteAsync(int id);
    }
}
