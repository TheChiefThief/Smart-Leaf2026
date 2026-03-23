using Npgsql;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Domain;

namespace SmartLeaf.Infrastructure.Repositories
{
    public class CareTaskRepository : ICareTaskRepository
    {
        private readonly string _connectionString;

        public CareTaskRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Supabase")!;
        }

        public async Task<IEnumerable<CareTask>> GetByPlantIdAsync(int plantId)
        {
            var tasks = new List<CareTask>();
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, plant_id, task_type, scheduled_date, status FROM care_tasks WHERE plant_id = @pid",
                conn);
            cmd.Parameters.AddWithValue("pid", plantId);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                tasks.Add(MapTask(reader));
            return tasks;
        }

        public async Task<CareTask?> GetByIdAsync(int id)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, plant_id, task_type, scheduled_date, status FROM care_tasks WHERE id = @id",
                conn);
            cmd.Parameters.AddWithValue("id", id);
            await using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapTask(reader) : null;
        }

        public async Task<CareTask> CreateAsync(CareTask task)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "INSERT INTO care_tasks (plant_id, task_type, scheduled_date, status) VALUES (@pid, @type, @date, @status) RETURNING id",
                conn);
            cmd.Parameters.AddWithValue("pid", task.PlantId);
            cmd.Parameters.AddWithValue("type", task.TaskType);
            cmd.Parameters.AddWithValue("date", task.ScheduledDate);
            cmd.Parameters.AddWithValue("status", task.Status);
            task.Id = (int)(long)(await cmd.ExecuteScalarAsync())!;
            return task;
        }

        public async Task UpdateStatusAsync(int id, string status)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "UPDATE care_tasks SET status = @status WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("status", status);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(CareTask task)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(
                "UPDATE care_tasks SET task_type = @type, scheduled_date = @date, status = @status WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("type", task.TaskType);
            cmd.Parameters.AddWithValue("date", task.ScheduledDate);
            cmd.Parameters.AddWithValue("status", task.Status);
            cmd.Parameters.AddWithValue("id", task.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand("DELETE FROM care_tasks WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        private static CareTask MapTask(NpgsqlDataReader r) => new()
        {
            Id            = (int)r.GetInt64(0),
            PlantId       = (int)r.GetInt64(1),
            TaskType      = r.GetString(2),
            ScheduledDate = r.IsDBNull(3) ? DateTime.UtcNow : r.GetDateTime(3),
            Status        = r.IsDBNull(4) ? string.Empty : r.GetString(4)
        };
    }
}
