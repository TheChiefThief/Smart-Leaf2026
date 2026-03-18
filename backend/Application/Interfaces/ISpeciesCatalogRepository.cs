using SmartLeaf.Domain;

namespace SmartLeaf.Application.Interfaces
{
    public interface ISpeciesCatalogRepository
    {
        Task<IEnumerable<SpeciesCatalog>> GetAllAsync();
        Task<SpeciesCatalog?> GetByIdAsync(int id);
        Task<IEnumerable<SpeciesCatalog>> SearchAsync(string query);
    }
}
