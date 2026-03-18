using System.Text;
using System.Text.Json;
using SmartLeaf.Application.DTOs;
using SmartLeaf.Application.Interfaces;

namespace SmartLeaf.Infrastructure.ExternalServices
{
    public class ChatbotService : IChatbotService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ChatbotService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["Chatbot:AccessToken"]
                ?? throw new InvalidOperationException("Chatbot:AccessToken missing in configuration");
        }

        public async Task<ChatbotResponse> AskAsync(ChatbotRequest request)
        {
            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";

            var response = await _httpClient.PostAsync(url, content);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Error al llamar a Gemini: {response.StatusCode}");

            var result = JsonSerializer.Deserialize<ChatbotResponse>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result == null || result.Candidates == null || result.Candidates.Count == 0)
                throw new Exception("Respuesta de Gemini inválida o sin candidatos.");

            return result;
        }
    }
}
