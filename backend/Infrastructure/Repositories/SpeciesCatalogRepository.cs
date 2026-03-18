using Npgsql;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Domain;

namespace SmartLeaf.Infrastructure.Repositories
{
    public class SpeciesCatalogRepository : ISpeciesCatalogRepository
    {
        private readonly string _connectionString;

        public SpeciesCatalogRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Supabase")!;
        }

        public async Task<IEnumerable<SpeciesCatalog>> GetAllAsync()
        {
            var list = new List<SpeciesCatalog>();
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, scientific_name, common_name, family, description, basic_care FROM species_catalog",
                conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(MapSpecies(reader));
            return list;
        }

        public async Task<SpeciesCatalog?> GetByIdAsync(int id)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, scientific_name, common_name, family, description, basic_care FROM species_catalog WHERE id = @id",
                conn);
            cmd.Parameters.AddWithValue("id", id);
            await using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapSpecies(reader) : null;
        }

        public async Task<IEnumerable<SpeciesCatalog>> SearchAsync(string query)
        {
            var list = new List<SpeciesCatalog>();
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                @"SELECT id, scientific_name, common_name, family, description, basic_care
                  FROM species_catalog
                  WHERE LOWER(common_name) LIKE @q OR LOWER(scientific_name) LIKE @q",
                conn);
            cmd.Parameters.AddWithValue("q", $"%{query.ToLower()}%");
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(MapSpecies(reader));
            return list;
        }

        private static SpeciesCatalog MapSpecies(NpgsqlDataReader r) => new()
        {
            Id             = (int)r.GetInt64(0),
            ScientificName = r.GetString(1),
            CommonName     = r.IsDBNull(2) ? string.Empty : r.GetString(2),
            Family         = r.IsDBNull(3) ? string.Empty : r.GetString(3),
            Description    = r.IsDBNull(4) ? string.Empty : r.GetString(4),
            BasicCare      = r.IsDBNull(5) ? string.Empty : r.GetString(5)
        };
    }
}
