using Npgsql;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Domain;

namespace SmartLeaf.Infrastructure.Repositories
{
    public class PlantRepository : IPlantRepository
    {
        private readonly string _connectionString;

        public PlantRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Supabase")!;
        }

        public async Task<IEnumerable<Plant>> GetByGardenIdAsync(int gardenId)
        {
            var plants = new List<Plant>();
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, garden_id, species_id, nickname, initial_quantity, planting_date, status, end_date, notes FROM plants WHERE garden_id = @gid",
                conn);
            cmd.Parameters.AddWithValue("gid", gardenId);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                plants.Add(MapPlant(reader));
            return plants;
        }

        public async Task<Plant?> GetByIdAsync(int id)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, garden_id, species_id, nickname, initial_quantity, planting_date, status, end_date, notes FROM plants WHERE id = @id",
                conn);
            cmd.Parameters.AddWithValue("id", id);
            await using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapPlant(reader) : null;
        }

        public async Task<Plant> CreateAsync(Plant plant)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                @"INSERT INTO plants (garden_id, species_id, nickname, initial_quantity, planting_date, status, notes)
                  VALUES (@gid, @sid, @nick, @qty, @pdate, @status, @notes) RETURNING id",
                conn);
            cmd.Parameters.AddWithValue("gid", plant.GardenId);
            cmd.Parameters.AddWithValue("sid", plant.SpeciesId);
            cmd.Parameters.AddWithValue("nick", plant.Nickname);
            cmd.Parameters.AddWithValue("qty", plant.InitialQuantity);
            cmd.Parameters.AddWithValue("pdate", plant.PlantingDate);
            cmd.Parameters.AddWithValue("status", plant.Status);
            cmd.Parameters.AddWithValue("notes", plant.Notes);
            plant.Id = (int)(long)(await cmd.ExecuteScalarAsync())!;
            return plant;
        }

        public async Task UpdateStatusAsync(int id, string status)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "UPDATE plants SET status = @status WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("status", status);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand("DELETE FROM plants WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        private static Plant MapPlant(NpgsqlDataReader r) => new()
        {
            Id              = (int)r.GetInt64(0),
            GardenId        = (int)r.GetInt64(1),
            SpeciesId       = (int)r.GetInt64(2),
            Nickname        = r.IsDBNull(3) ? string.Empty : r.GetString(3),
            InitialQuantity = r.IsDBNull(4) ? 0 : r.GetInt32(4),
            PlantingDate    = r.IsDBNull(5) ? DateTime.UtcNow : r.GetDateTime(5),
            Status          = r.IsDBNull(6) ? string.Empty : r.GetString(6),
            EndDate         = r.IsDBNull(7) ? null : r.GetDateTime(7),
            Notes           = r.IsDBNull(8) ? string.Empty : r.GetString(8)
        };
    }
}
