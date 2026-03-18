using Newtonsoft.Json.Linq;
using SmartLeaf.Application.Interfaces;

namespace SmartLeaf.Infrastructure.ExternalServices
{
    public class PlantSearchService : IPlantSearchService
    {
        private readonly HttpClient _httpClient;

        public PlantSearchService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> GetPlantImageUrlAsync(string plantName)
        {
            var encodedName = System.Net.WebUtility.UrlEncode(plantName);
            var url = $"https://api.inaturalist.org/v1/search?q={encodedName}&sources=taxa";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var results = json["results"];
            if (results == null || !results.HasValues) return null;

            return results[0]?["record"]?["default_photo"]?["medium_url"]?.ToString();
        }
    }
}
