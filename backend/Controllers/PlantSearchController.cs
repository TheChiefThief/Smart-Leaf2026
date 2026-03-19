using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLeaf.Application.Interfaces;

namespace SmartLeaf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlantSearchController : ControllerBase
    {
        private readonly IPlantSearchService _plantSearchService;

        public PlantSearchController(IPlantSearchService plantSearchService) =>
            _plantSearchService = plantSearchService;

        /// <summary>
        /// Busca la URL de imagen de una planta por nombre.
        /// </summary>
        [HttpGet("{name}")]
        public async Task<IActionResult> GetPlantImage(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "El nombre no puede estar vacío." });

            var photoUrl = await _plantSearchService.GetPlantImageUrlAsync(name);
            if (string.IsNullOrEmpty(photoUrl))
                return NotFound(new { message = "No se encontró imagen para esa planta." });

            return Ok(new { imageUrl = photoUrl });
        }
    }
}
