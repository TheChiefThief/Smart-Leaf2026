using Npgsql;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Domain;

namespace SmartLeaf.Infrastructure.Repositories
{
    public class GardenRepository : IGardenRepository
    {
        private readonly string _connectionString;

        public GardenRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Supabase")!;
        }

        public async Task<IEnumerable<Garden>> GetByUserIdAsync(string userId)
        {
            var gardens = new List<Garden>();
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, user_id, name, zone_id, soil_type_id, sun_exposure FROM gardens WHERE user_id = @uid::uuid",
                conn);
            cmd.Parameters.AddWithValue("uid", userId);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                gardens.Add(MapGarden(reader));
            return gardens;
        }

        public async Task<Garden?> GetByIdAsync(int id)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, user_id, name, zone_id, soil_type_id, sun_exposure FROM gardens WHERE id = @id",
                conn);
            cmd.Parameters.AddWithValue("id", id);
            await using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapGarden(reader) : null;
        }

        public async Task<Garden> CreateAsync(Garden garden)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                @"INSERT INTO gardens (user_id, name, zone_id, soil_type_id, sun_exposure)
                  VALUES (@uid::uuid, @name, @zone, @soil, @sun) RETURNING id",
                conn);
            cmd.Parameters.AddWithValue("uid", garden.UserId);
            cmd.Parameters.AddWithValue("name", garden.Name);
            cmd.Parameters.AddWithValue("zone", garden.ClimateZoneId == 0 ? (object)DBNull.Value : (object)garden.ClimateZoneId);
            cmd.Parameters.AddWithValue("soil", garden.SoilTypeId == 0 ? (object)DBNull.Value : (object)garden.SoilTypeId);
            cmd.Parameters.AddWithValue("sun", garden.SunExposure ?? (object)DBNull.Value);
            garden.Id = (int)(long)(await cmd.ExecuteScalarAsync())!;
            return garden;
        }

        public async Task DeleteAsync(int id)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand("DELETE FROM gardens WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        private static Garden MapGarden(NpgsqlDataReader r) => new()
        {
            Id            = (int)r.GetInt64(0),
            UserId        = r.GetGuid(1).ToString(),
            Name          = r.GetString(2),
            ClimateZoneId = r.IsDBNull(3) ? 0 : (int)r.GetInt64(3),
            SoilTypeId    = r.IsDBNull(4) ? 0 : (int)r.GetInt64(4),
            SunExposure   = r.IsDBNull(5) ? string.Empty : r.GetString(5)
        };
    }
}
