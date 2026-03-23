using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLeaf.Application.DTOs;
using SmartLeaf.Application.Interfaces;

namespace SmartLeaf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TerrainController : ControllerBase
    {
        private readonly ITerrainService _service;

        public TerrainController(ITerrainService service) => _service = service;

        // ── Terrain ──────────────────────────────────────────────────────────

        /// <summary>
        /// Devuelve el terreno de un jardín con todos sus sprouts.
        /// Un solo GET carga todo el canvas 2D.
        /// </summary>
        [HttpGet("garden/{gardenId}")]
        public async Task<IActionResult> GetByGarden(int gardenId)
        {
            var terrain = await _service.GetByGardenIdAsync(gardenId);
            if (terrain == null) return NotFound(new { message = "Terreno no encontrado para este jardín." });
            return Ok(terrain);
        }

        /// <summary>
        /// Crea el terreno de un jardín (existente uno por jardín).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTerrainRequest request)
        {
            var terrain = await _service.CreateAsync(request);
            return Ok(terrain);
        }

        /// <summary>
        /// Actualiza la forma o dimensiones del lienzo.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTerrainRequest request)
        {
            await _service.UpdateAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// Elimina el terreno (y en cascada todos sus sprouts, si la DB lo permite).
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // ── Sprouts ──────────────────────────────────────────────────────────

        /// <summary>
        /// Agrega una planta posicionada al lienzo.
        /// </summary>
        [HttpPost("sprout")]
        public async Task<IActionResult> AddSprout([FromBody] CreateSproutRequest request)
        {
            var sprout = await _service.AddSproutAsync(request);
            return Ok(sprout);
        }

        /// <summary>
        /// Actualiza la posición o el estilo de un sprout (permite drag-and-drop desde el front).
        /// </summary>
        [HttpPut("sprout/{sproutId}")]
        public async Task<IActionResult> UpdateSprout(int sproutId, [FromBody] UpdateSproutRequest request)
        {
            await _service.UpdateSproutAsync(sproutId, request);
            return NoContent();
        }

        /// <summary>
        /// Elimina un sprout del lienzo.
        /// </summary>
        [HttpDelete("sprout/{sproutId}")]
        public async Task<IActionResult> DeleteSprout(int sproutId)
        {
            await _service.DeleteSproutAsync(sproutId);
            return NoContent();
        }
    }
}
