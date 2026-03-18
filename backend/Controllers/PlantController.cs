using Microsoft.AspNetCore.Mvc;
using SmartLeaf.Application.Interfaces;

namespace SmartLeaf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlantController : ControllerBase
    {
        private readonly IPlantIdentificationService _identificationService;

        public PlantController(IPlantIdentificationService identificationService) =>
            _identificationService = identificationService;

        [HttpPost("identify")]
        public async Task<IActionResult> IdentifyPlant([FromBody] string base64Image)
        {
            if (string.IsNullOrEmpty(base64Image))
                return BadRequest("La imagen no puede estar vacía.");

            var result = await _identificationService.IdentifyPlantAsync(base64Image);
            return Ok(result);
        }
    }
}