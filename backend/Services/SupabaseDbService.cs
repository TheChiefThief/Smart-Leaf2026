using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;
using System;

namespace data.API
{
    public class SupabaseDbService
    {
        private readonly string _connectionString;

        public SupabaseDbService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Supabase");
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();
                await conn.CloseAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] No se pudo conectar a Supabase: {ex.Message}");
                return false;
            }
        }

        // Ejemplo: obtener el conteo de una tabla
        public async Task<int> GetTableCountAsync(string tableName)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand($"SELECT COUNT(*) FROM \"{tableName}\"", conn);
            var count = (long)await cmd.ExecuteScalarAsync();
            return (int)count;
        }
    }
}