namespace SmartLeaf.Application.Interfaces
{
    /// <summary>
    /// Abstracción para la API REST de Supabase Auth (auth.users bajo el capó).
    /// No accede a la DB directamente — llama a https://[project].supabase.co/auth/v1
    /// </summary>
    public interface ISupabaseAuthProvider
    {
        Task<SupabaseSignUpResult?> SignUpAsync(string email, string password, string username, string fullName);
        Task<SupabaseSignInResult?> SignInAsync(string email, string password);
        Task PatchProfileAsync(string userId, string accessToken, string username, string fullName);
    }

    public class SupabaseSignUpResult
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? AccessToken { get; set; }
    }

    public class SupabaseSignInResult
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
