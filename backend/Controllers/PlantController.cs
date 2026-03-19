using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLeaf.Application.Interfaces;

namespace SmartLeaf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlantController : ControllerBase
    {
        private readonly IPlantIdentificationService _identificationService;

        public PlantController(IPlantIdentificationService identificationService) =>
            _identificationService = identificationService;

        /// <summary>
        /// Identifica una planta a partir de una imagen en base64.
        /// Requiere autenticación JWT.
        /// </summary>
        [HttpPost("identify")]
        public async Task<IActionResult> IdentifyPlant([FromBody] string base64Image)
        {
            if (string.IsNullOrWhiteSpace(base64Image))
                return BadRequest(new { message = "La imagen no puede estar vacía." });

            var result = await _identificationService.IdentifyPlantAsync(base64Image);
            return Ok(result);
        }
    }
}