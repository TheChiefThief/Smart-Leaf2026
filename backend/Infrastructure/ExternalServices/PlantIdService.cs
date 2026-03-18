using System.Text;
using System.Text.Json;
using SmartLeaf.Application.Interfaces;

namespace SmartLeaf.Infrastructure.ExternalServices
{
    public class PlantIdService : IPlantIdentificationService
    {
        private readonly HttpClient _httpClient;
        private readonly List<string> _apiKeys;
        private int _currentApiKeyIndex = 0;
        private const string _url = "https://api.plant.id/v2/identify";

        public PlantIdService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKeys = config.GetSection("PlantId:ApiKeys").Get<List<string>>() ?? new List<string>();
        }

        public async Task<string> IdentifyPlantAsync(string base64Image)
        {
            while (_currentApiKeyIndex < _apiKeys.Count)
            {
                try
                {
                    var currentApiKey = _apiKeys[_currentApiKeyIndex];
                    _httpClient.DefaultRequestHeaders.Remove("Api-Key");
                    _httpClient.DefaultRequestHeaders.Add("Api-Key", currentApiKey);

                    var requestBody = new
                    {
                        images = new[] { $"data:image/jpg;base64,{base64Image}" },
                        organs = new[] { "leaf" },
                        modifiers = new[] { "crops_fast", "similar_images" },
                        plant_language = "es",
                        plant_details = new[] { "common_names", "url", "name_authority", "wiki_description" }
                    };

                    var json = JsonSerializer.Serialize(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync(_url, content);

                    if (response.IsSuccessStatusCode)
                        return await response.Content.ReadAsStringAsync();

                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[ERROR] Plant.id API: {response.StatusCode} - {errorContent}");

                    if (response.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                        errorContent.Contains("API key is invalid") ||
                        errorContent.Contains("quota exceeded"))
                    {
                        _currentApiKeyIndex++;
                    }
                    else
                    {
                        throw new HttpRequestException($"Error en Plant.id: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] PlantIdService: {ex.Message}");
                    _currentApiKeyIndex++;
                }
            }
            throw new InvalidOperationException("No quedan API Keys de Plant.id disponibles.");
        }
    }
}
