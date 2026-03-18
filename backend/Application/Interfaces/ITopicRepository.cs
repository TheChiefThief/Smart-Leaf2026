using SmartLeaf.Domain;

namespace SmartLeaf.Application.Interfaces
{
    public interface ITopicRepository
    {
        Task<IEnumerable<Topic>> GetAllAsync();
        Task<Topic?> GetByIdAsync(int id);
    }
}
