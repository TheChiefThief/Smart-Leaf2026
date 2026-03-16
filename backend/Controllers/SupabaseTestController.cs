using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using data.API;

namespace presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupabaseTestController : ControllerBase
    {
        private readonly SupabaseDbService _dbService;

        public SupabaseTestController(SupabaseDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("test-connection")]
        public async Task<IActionResult> TestConnection()
        {
            var ok = await _dbService.TestConnectionAsync();
            return ok ? Ok("Conexión exitosa a Supabase!") : StatusCode(500, "Error de conexión.");
        }

        [HttpGet("count/{table}")]
        public async Task<IActionResult> GetTableCount(string table)
        {
            try
            {
                var count = await _dbService.GetTableCountAsync(table);
                return Ok(new { table, count });
            }
            catch
            {
                return StatusCode(500, "Error al consultar la tabla.");
            }
        }
    }
}