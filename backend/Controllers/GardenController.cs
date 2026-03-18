using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLeaf.Application.DTOs;
using SmartLeaf.Application.Interfaces;
using System.Security.Claims;

namespace SmartLeaf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GardenController : ControllerBase
    {
        private readonly IGardenService _gardenService;

        public GardenController(IGardenService gardenService) => _gardenService = gardenService;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetMyGardens()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var gardens = await _gardenService.GetUserGardensAsync(userId);
            return Ok(gardens);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGardenRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var garden = await _gardenService.CreateGardenAsync(userId, request);
            return Ok(garden);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _gardenService.DeleteGardenAsync(id);
            return NoContent();
        }
    }
}
