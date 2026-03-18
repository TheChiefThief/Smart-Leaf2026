using Npgsql;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Domain;

namespace SmartLeaf.Infrastructure.Repositories
{
    /// <summary>
    /// Solo maneja la tabla public.profile.
    /// La creación del usuario en auth.users la gestiona SupabaseAuthProvider.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Supabase")!;
        }

        public async Task<Profile?> GetByIdAsync(string userId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, email, username, nombre_completo, avatar_url, biografia, es_privado FROM profile WHERE id = @id::uuid",
                conn);
            cmd.Parameters.AddWithValue("id", userId);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;
            return MapProfile(reader);
        }

        public async Task<Profile?> GetByEmailAsync(string email)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, email, username, nombre_completo, avatar_url, biografia, es_privado FROM profile WHERE email = @email",
                conn);
            cmd.Parameters.AddWithValue("email", email);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;
            return MapProfile(reader);
        }

        public async Task CreateProfileAsync(Profile profile)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                @"INSERT INTO profile (id, email, username, nombre_completo, avatar_url, biografia, es_privado)
                  VALUES (@id::uuid, @email, @username, @nombre, @avatar, @bio, @priv)
                  ON CONFLICT (id) DO NOTHING",
                conn);
            cmd.Parameters.AddWithValue("id", profile.Id);
            cmd.Parameters.AddWithValue("email", profile.Email);
            cmd.Parameters.AddWithValue("username", profile.Username);
            cmd.Parameters.AddWithValue("nombre", profile.NombreCompleto);
            cmd.Parameters.AddWithValue("avatar", (object?)profile.AvatarUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("bio", (object?)profile.Biografia ?? DBNull.Value);
            cmd.Parameters.AddWithValue("priv", profile.EsPrivado);
            await cmd.ExecuteNonQueryAsync();
        }

        private static Profile MapProfile(NpgsqlDataReader r) => new()
        {
            Id            = r.GetString(0),
            Email         = r.GetString(1),
            Username      = r.GetString(2),
            NombreCompleto = r.IsDBNull(3) ? string.Empty : r.GetString(3),
            AvatarUrl     = r.IsDBNull(4) ? null : r.GetString(4),
            Biografia     = r.IsDBNull(5) ? null : r.GetString(5),
            EsPrivado     = r.GetBoolean(6)
        };
    }
}
