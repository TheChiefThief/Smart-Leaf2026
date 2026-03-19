using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SmartLeaf.Application.DTOs;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Domain;

namespace SmartLeaf.Services
{
    public class AuthService : IAuthService
    {
        private readonly ISupabaseAuthProvider _supabaseAuth;
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public AuthService(
            ISupabaseAuthProvider supabaseAuth,
            IUserRepository userRepo,
            IConfiguration config)
        {
            _supabaseAuth = supabaseAuth;
            _userRepo     = userRepo;
            _config       = config;
        }

        public async Task<(bool Success, string Error)> RegisterAsync(RegisterRequest request)
        {
            // Supabase Auth crea el usuario en auth.users (pasamos user_meta_data)
            var result = await _supabaseAuth.SignUpAsync(request.Email, request.Password, request.UserName, request.FullName);
            if (result == null)
                return (false, "No se pudo crear el usuario en Supabase Auth.");

            // Si el trigger no toma la metadata correctamente, forzamos un PATCH
            if (!string.IsNullOrEmpty(result.AccessToken))
            {
                await Task.Delay(500); // Dar 500ms al trigger para crear la fila en profile
                await _supabaseAuth.PatchProfileAsync(result.UserId, result.AccessToken, request.UserName, request.FullName);
            }

            return (true, string.Empty);
        }

        public async Task<string?> AuthenticateAsync(string email, string password)
        {
            // 1. Autenticar contra Supabase Auth
            var result = await _supabaseAuth.SignInAsync(email, password);
            if (result == null) return null;

            // 2. Emitir nuestro propio JWT con el UUID del usuario
            return GenerateJwtToken(result.UserId, result.Email);
        }

        private string GenerateJwtToken(string userId, string email)
        {
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role, "User")
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
