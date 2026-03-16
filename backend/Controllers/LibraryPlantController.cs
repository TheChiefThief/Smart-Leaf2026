using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLeaf.Domain;
using System.Security.Claims;
using Npgsql;

namespace SmartLeaf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryPlantController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public LibraryPlantController(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("Supabase");
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddPlant([FromBody] PlantRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine($"UserId que se va a insertar en plants: {userId}");
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var cmd = new NpgsqlCommand(
                @"INSERT INTO smartleafdb.plants 
                (""UserId"", ""PlantTypeId"", ""CustomName"", ""PlantedDate"", ""Region"", ""Status"") 
                VALUES (@uid, @ptid, @cname, @pdate, @region, @status)", conn))
            {
                cmd.Parameters.AddWithValue("uid", userId);
                cmd.Parameters.AddWithValue("ptid", request.PlantTypeId);
                cmd.Parameters.AddWithValue("cname", request.CustomName ?? "");
                cmd.Parameters.AddWithValue("pdate", request.PlantedDate);
                cmd.Parameters.AddWithValue("region", request.Region ?? "");
                cmd.Parameters.AddWithValue("status", request.Status);
                await cmd.ExecuteNonQueryAsync();
            }

            return Ok(new { message = "Planta agregada correctamente" });
        }

        [Authorize]
        [HttpGet("myplants")]
        public async Task<IActionResult> GetMyPlants()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var plants = new List<Plant>();

            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using (var cmd = new NpgsqlCommand(
                @"SELECT ""Id"", ""UserId"", ""PlantTypeId"", ""CustomName"", ""PlantedDate"", ""Region"", ""Status"" 
                  FROM smartleafdb.plants WHERE ""UserId"" = @uid", conn))
            {
                cmd.Parameters.AddWithValue("uid", userId);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    plants.Add(new Plant
                    {
                        Id = reader.GetInt32(0),
                        UserId = reader.GetString(1),
                        PlantTypeId = reader.GetInt32(2),
                        CustomName = reader.GetString(3),
                        PlantedDate = reader.GetDateTime(4),
                        Region = reader.GetString(5),
                        Status = reader.GetInt32(6)
                    });
                }
            }

            return Ok(plants);
        }
    }
}