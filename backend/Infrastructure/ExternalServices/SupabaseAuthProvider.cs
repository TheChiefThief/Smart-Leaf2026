using System.Text;
using System.Text.Json;
using SmartLeaf.Application.Interfaces;

namespace SmartLeaf.Infrastructure.ExternalServices
{
    public class SupabaseAuthProvider : ISupabaseAuthProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _supabaseUrl;
        private readonly string _anonKey;

        public SupabaseAuthProvider(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _supabaseUrl = config["Supabase:Url"]
                ?? throw new InvalidOperationException("Supabase:Url missing in configuration");
            _anonKey = config["Supabase:AnonKey"]
                ?? throw new InvalidOperationException("Supabase:AnonKey missing in configuration");
        }

        public async Task<SupabaseSignUpResult?> SignUpAsync(string email, string password)
        {
            var url = $"{_supabaseUrl}/auth/v1/signup";
            var body = JsonSerializer.Serialize(new { email, password });
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("apikey", _anonKey);
            request.Headers.Add("Authorization", $"Bearer {_anonKey}");

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"[Supabase Auth] SignUp status: {response.StatusCode}");
            Console.WriteLine($"[Supabase Auth] SignUp response: {json}");

            if (!response.IsSuccessStatusCode)
                return null;

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Caso 1: { "user": { "id": "...", "email": "..." }, "session": ... }
            if (root.TryGetProperty("user", out var userEl) &&
                userEl.ValueKind == JsonValueKind.Object &&
                userEl.TryGetProperty("id", out var idFromUser))
            {
                return new SupabaseSignUpResult
                {
                    UserId = idFromUser.GetString() ?? string.Empty,
                    Email  = userEl.TryGetProperty("email", out var emEl) ? emEl.GetString() ?? email : email
                };
            }

            // Caso 2: user devuelto directamente en el root { "id": "...", "email": "..." }
            if (root.TryGetProperty("id", out var idEl))
            {
                return new SupabaseSignUpResult
                {
                    UserId = idEl.GetString() ?? string.Empty,
                    Email  = root.TryGetProperty("email", out var emEl2) ? emEl2.GetString() ?? email : email
                };
            }

            Console.WriteLine("[Supabase Auth] No se encontró el campo 'id' en la respuesta.");
            return null;
        }

        public async Task<SupabaseSignInResult?> SignInAsync(string email, string password)
        {
            var url = $"{_supabaseUrl}/auth/v1/token?grant_type=password";
            var body = JsonSerializer.Serialize(new { email, password });
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("apikey", _anonKey);

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[Supabase Auth] SignIn error: {response.StatusCode} - {json}");
                return null;
            }

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            var userEl = root.GetProperty("user");

            return new SupabaseSignInResult
            {
                UserId = userEl.GetProperty("id").GetString() ?? string.Empty,
                Email  = userEl.GetProperty("email").GetString() ?? string.Empty
            };
        }
    }
}
