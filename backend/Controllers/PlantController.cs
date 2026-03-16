using Microsoft.AspNetCore.Mvc;
using data.API; 
using System.Threading.Tasks;

namespace presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlantController : ControllerBase
    {
        private readonly PlantIdService _plantIdService;

        public PlantController(PlantIdService plantIdService)
        {
            _plantIdService = plantIdService;
        }

        [HttpPost("identify")]
        public async Task<IActionResult> IdentifyPlant([FromBody] string base64Image)
        {
            if (string.IsNullOrEmpty(base64Image))
            {
                return BadRequest("La imagen no puede estar vacía.");
            }

            var result = await _plantIdService.IdentifyPlantAsync(base64Image);
            return Ok(result);
        }
    }
}