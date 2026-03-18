namespace SmartLeaf.Application.Interfaces
{
    public interface IPlantSearchService
    {
        Task<string?> GetPlantImageUrlAsync(string plantName);
    }
}
