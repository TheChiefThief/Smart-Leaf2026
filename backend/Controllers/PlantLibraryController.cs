using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLeaf.Application.DTOs;
using SmartLeaf.Application.Interfaces;

namespace SmartLeaf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlantLibraryController : ControllerBase
    {
        private readonly IPlantService _plantService;

        public PlantLibraryController(IPlantService plantService) => _plantService = plantService;

        [Authorize]
        [HttpGet("garden/{gardenId}")]
        public async Task<IActionResult> GetByGarden(int gardenId)
        {
            var plants = await _plantService.GetByGardenIdAsync(gardenId);
            return Ok(plants);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var plant = await _plantService.GetPlantByIdAsync(id);
            if (plant == null) return NotFound(new { message = "Planta no encontrada." });
            return Ok(plant);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreatePlantRequest request)
        {
            var plant = await _plantService.AddPlantAsync(request);
            return Ok(plant);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePlantRequest request)
        {
            await _plantService.UpdatePlantAsync(id, request);
            return NoContent();
        }

        [Authorize]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            await _plantService.UpdateStatusAsync(id, status);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _plantService.DeletePlantAsync(id);
            return NoContent();
        }
    }
}
