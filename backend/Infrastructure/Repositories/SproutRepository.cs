using Npgsql;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Domain;

namespace SmartLeaf.Infrastructure.Repositories
{
    public class SproutRepository : ISproutRepository
    {
        private readonly string _conn;

        public SproutRepository(IConfiguration config)
            => _conn = config.GetConnectionString("Supabase")!;

        public async Task<IEnumerable<Sprout>> GetByTerrainIdAsync(int terrainId)
        {
            var list = new List<Sprout>();
            await using var conn = new NpgsqlConnection(_conn);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, plant_id, terrain_id, label, color, form, x, y, z FROM sprout WHERE terrain_id = @tid",
                conn);
            cmd.Parameters.AddWithValue("tid", terrainId);
            await using var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync())
                list.Add(Map(r));
            return list;
        }

        public async Task<Sprout?> GetByIdAsync(int id)
        {
            await using var conn = new NpgsqlConnection(_conn);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, plant_id, terrain_id, label, color, form, x, y, z FROM sprout WHERE id = @id",
                conn);
            cmd.Parameters.AddWithValue("id", id);
            await using var r = await cmd.ExecuteReaderAsync();
            return await r.ReadAsync() ? Map(r) : null;
        }

        public async Task<Sprout> CreateAsync(Sprout sprout)
        {
            await using var conn = new NpgsqlConnection(_conn);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                @"INSERT INTO sprout (plant_id, terrain_id, label, color, form, x, y, z)
                  VALUES (@pid, @tid, @label, @color, @form, @x, @y, @z) RETURNING id",
                conn);
            cmd.Parameters.AddWithValue("pid",   sprout.PlantId);
            cmd.Parameters.AddWithValue("tid",   sprout.TerrainId);
            cmd.Parameters.AddWithValue("label", sprout.Label);
            cmd.Parameters.AddWithValue("color", sprout.Color);
            cmd.Parameters.AddWithValue("form",  sprout.Form);
            cmd.Parameters.AddWithValue("x",     sprout.X);
            cmd.Parameters.AddWithValue("y",     sprout.Y);
            cmd.Parameters.AddWithValue("z",     sprout.Z);
            sprout.Id = (int)(long)(await cmd.ExecuteScalarAsync())!;
            return sprout;
        }

        public async Task UpdateAsync(Sprout sprout)
        {
            await using var conn = new NpgsqlConnection(_conn);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                @"UPDATE sprout SET label = @label, color = @color, form = @form,
                  x = @x, y = @y, z = @z WHERE id = @id",
                conn);
            cmd.Parameters.AddWithValue("label", sprout.Label);
            cmd.Parameters.AddWithValue("color", sprout.Color);
            cmd.Parameters.AddWithValue("form",  sprout.Form);
            cmd.Parameters.AddWithValue("x",     sprout.X);
            cmd.Parameters.AddWithValue("y",     sprout.Y);
            cmd.Parameters.AddWithValue("z",     sprout.Z);
            cmd.Parameters.AddWithValue("id",    sprout.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await using var conn = new NpgsqlConnection(_conn);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand("DELETE FROM sprout WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        private static Sprout Map(NpgsqlDataReader r) => new()
        {
            Id        = (int)r.GetInt64(0),
            PlantId   = (int)r.GetInt64(1),
            TerrainId = (int)r.GetInt64(2),
            Label     = r.IsDBNull(3) ? string.Empty : r.GetString(3),
            Color     = r.IsDBNull(4) ? string.Empty : r.GetString(4),
            Form      = r.IsDBNull(5) ? string.Empty : r.GetString(5),
            X         = r.IsDBNull(6) ? 0 : r.GetDouble(6),
            Y         = r.IsDBNull(7) ? 0 : r.GetDouble(7),
            Z         = r.IsDBNull(8) ? 0 : r.GetDouble(8),
        };
    }
}
