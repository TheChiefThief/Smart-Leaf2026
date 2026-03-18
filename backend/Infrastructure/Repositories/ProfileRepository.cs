using Npgsql;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Domain;

namespace SmartLeaf.Infrastructure.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly string _connectionString;

        public ProfileRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Supabase")!;
        }

        public async Task<Profile?> GetByUserIdAsync(string userId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, email, username, nombre_completo, avatar_url, biografia, es_privado FROM profile WHERE id = @id::uuid",
                conn);
            cmd.Parameters.AddWithValue("id", userId);
            await using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapProfile(reader) : null;
        }

        public async Task<Profile> CreateAsync(Profile profile)
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
            return profile;
        }

        public async Task UpdateAsync(Profile profile)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                @"UPDATE profile
                  SET nombre_completo = @nombre, avatar_url = @avatar, biografia = @bio, es_privado = @priv
                  WHERE id = @id::uuid",
                conn);
            cmd.Parameters.AddWithValue("nombre", profile.NombreCompleto);
            cmd.Parameters.AddWithValue("avatar", (object?)profile.AvatarUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("bio", (object?)profile.Biografia ?? DBNull.Value);
            cmd.Parameters.AddWithValue("priv", profile.EsPrivado);
            cmd.Parameters.AddWithValue("id", profile.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        private static Profile MapProfile(NpgsqlDataReader r) => new()
        {
            Id             = r.GetGuid(0).ToString(),
            Email          = r.GetString(1),
            Username       = r.GetString(2),
            NombreCompleto = r.IsDBNull(3) ? string.Empty : r.GetString(3),
            AvatarUrl      = r.IsDBNull(4) ? null : r.GetString(4),
            Biografia      = r.IsDBNull(5) ? null : r.GetString(5),
            EsPrivado      = r.GetBoolean(6)
        };
    }
}
