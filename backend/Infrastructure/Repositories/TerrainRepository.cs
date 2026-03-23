using Npgsql;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Domain;
using System.Text.Json;

namespace SmartLeaf.Infrastructure.Repositories
{
    public class TerrainRepository : ITerrainRepository
    {
        private readonly string _conn;

        public TerrainRepository(IConfiguration config)
            => _conn = config.GetConnectionString("Supabase")!;

        public async Task<Terrain?> GetByGardenIdAsync(int gardenId)
        {
            await using var conn = new NpgsqlConnection(_conn);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, garden_id, shape_type, dimensions, created_at FROM terrain WHERE garden_id = @gid",
                conn);
            cmd.Parameters.AddWithValue("gid", gardenId);
            await using var r = await cmd.ExecuteReaderAsync();
            return await r.ReadAsync() ? Map(r) : null;
        }

        public async Task<Terrain?> GetByIdAsync(int id)
        {
            await using var conn = new NpgsqlConnection(_conn);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, garden_id, shape_type, dimensions, created_at FROM terrain WHERE id = @id",
                conn);
            cmd.Parameters.AddWithValue("id", id);
            await using var r = await cmd.ExecuteReaderAsync();
            return await r.ReadAsync() ? Map(r) : null;
        }

        public async Task<Terrain> CreateAsync(Terrain terrain)
        {
            await using var conn = new NpgsqlConnection(_conn);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                @"INSERT INTO terrain (garden_id, shape_type, dimensions)
                  VALUES (@gid, @shape, @dims::jsonb) RETURNING id, created_at",
                conn);
            cmd.Parameters.AddWithValue("gid", terrain.GardenId);
            cmd.Parameters.AddWithValue("shape", terrain.ShapeType);
            cmd.Parameters.AddWithValue("dims",
                terrain.Dimensions != null ? terrain.Dimensions.RootElement.ToString() : (object)DBNull.Value);

            await using var r = await cmd.ExecuteReaderAsync();
            if (await r.ReadAsync())
            {
                terrain.Id = (int)r.GetInt64(0);
                terrain.CreatedAt = r.GetDateTime(1);
            }
            return terrain;
        }

        public async Task UpdateAsync(Terrain terrain)
        {
            await using var conn = new NpgsqlConnection(_conn);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "UPDATE terrain SET shape_type = @shape, dimensions = @dims::jsonb WHERE id = @id",
                conn);
            cmd.Parameters.AddWithValue("shape", terrain.ShapeType);
            cmd.Parameters.AddWithValue("dims",
                terrain.Dimensions != null ? terrain.Dimensions.RootElement.ToString() : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("id", terrain.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await using var conn = new NpgsqlConnection(_conn);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand("DELETE FROM terrain WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        private static Terrain Map(NpgsqlDataReader r) => new()
        {
            Id         = (int)r.GetInt64(0),
            GardenId   = (int)r.GetInt64(1),
            ShapeType  = r.GetString(2),
            Dimensions = r.IsDBNull(3) ? null : JsonDocument.Parse(r.GetString(3)),
            CreatedAt  = r.GetDateTime(4)
        };
    }
}
