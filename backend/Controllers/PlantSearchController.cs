using Microsoft.AspNetCore.Mvc;
using SmartLeaf.Application.Interfaces;

namespace SmartLeaf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlantSearchController : ControllerBase
    {
        private readonly IPlantSearchService _plantSearchService;

        public PlantSearchController(IPlantSearchService plantSearchService) =>
            _plantSearchService = plantSearchService;

        [HttpGet("{name}")]
        public async Task<IActionResult> GetPlantImage(string name)
        {
            var photoUrl = await _plantSearchService.GetPlantImageUrlAsync(name);
            if (string.IsNullOrEmpty(photoUrl))
                return NotFound("No se encontró imagen para esa planta.");
            return Ok(photoUrl);
        }
    }
}
