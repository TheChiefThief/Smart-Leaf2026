using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace data.API
{
    public class PlantIdService
    {
        private readonly HttpClient _httpClient;
        private readonly List<string> _apiKeys = new()
        {
            "Lq1gU81VpzPVQMvSmTm1ZHOLiPZuhFBCBYvtEUzzXO7IaIvDm2", // Primera API Key - Mariano
            "wIbuKl58vQx2VUhRbjvk2UGRsoc3FmC0raFAbNj4lssh0CsXPk", // Segunda API Key - Mariano
            "38uu4UOHxF9WBPA6ABjuGCKT5kuhAYsKP2HfIlKJaKjmTIV7Zy"  // Tercera API Key - Luciano
        };
        private int _currentApiKeyIndex = 0; // Índice de la API Key actual
        private const string _url = "https://api.plant.id/v2/identify";

        public PlantIdService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_url);
        }

        public async Task<string> IdentifyPlantAsync(string base64Image)
        {
            while (_currentApiKeyIndex < _apiKeys.Count)
            {
                try
                {
                    // Configurar la API Key actual
                    var currentApiKey = _apiKeys[_currentApiKeyIndex];
                    _httpClient.DefaultRequestHeaders.Remove("Api-Key");
                    _httpClient.DefaultRequestHeaders.Add("Api-Key", currentApiKey);

                    // Crear el cuerpo de la solicitud
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

                    // Enviar la solicitud
                    var response = await _httpClient.PostAsync(_url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Si la respuesta es exitosa, devolver el contenido
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        // Leer el contenido del error
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"[ERROR] Respuesta de la API: {response.StatusCode} - {errorContent}");

                        // Si el error indica que la API Key no es válida o no tiene más consultas, cambiar a la siguiente
                        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden || 
                            errorContent.Contains("API key is invalid") || 
                            errorContent.Contains("quota exceeded"))
                        {
                            Console.WriteLine($"[INFO] Cambiando a la siguiente API Key...");
                            _currentApiKeyIndex++;
                        }
                        else
                        {
                            // Si el error no está relacionado con la API Key, lanzar una excepción
                            throw new HttpRequestException($"Error en la solicitud: {response.StatusCode} - {errorContent}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Error al identificar la planta: {ex.Message}");
                    _currentApiKeyIndex++;
                }
            }

            // Si se agotaron todas las API Keys, lanzar una excepción
            throw new InvalidOperationException("No quedan API Keys disponibles o todas fallaron.");
        }
    }
}