using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class ChatbotService
{
    private readonly HttpClient _httpClient;

    public ChatbotService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ChatbotResponse> AskChatbotAsync(string accessToken, ChatbotRequest request)
    {
        // Serializar el request a JSON en camelCase
        var jsonRequest = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        Console.WriteLine($"[DEBUG] Cuerpo enviado a la API externa:\n{jsonRequest}");

        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={accessToken}";

        var response = await _httpClient.PostAsync(url, content);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"[DEBUG] Respuesta cruda recibida:\n{jsonResponse}");

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"[ERROR] Código de estado: {response.StatusCode}");
            throw new HttpRequestException($"Error al llamar al servicio externo: {response.StatusCode}");
        }

        var chatbotResponse = JsonSerializer.Deserialize<ChatbotResponse>(jsonResponse, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (chatbotResponse == null || chatbotResponse.Candidates == null || chatbotResponse.Candidates.Count == 0)
        {
            Console.WriteLine("[ERROR] No se pudieron deserializar los candidatos correctamente o la lista está vacía.");
            throw new Exception("La respuesta del chatbot es nula o no contiene candidatos.");
        }

        return chatbotResponse;
    }
}
