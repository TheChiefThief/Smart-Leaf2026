namespace SmartLeaf.Application.Interfaces
{
    public interface IPlantIdentificationService
    {
        Task<string> IdentifyPlantAsync(string base64Image);
    }
}
