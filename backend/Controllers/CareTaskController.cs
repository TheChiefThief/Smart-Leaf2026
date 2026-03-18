using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLeaf.Application.DTOs;
using SmartLeaf.Application.Interfaces;
using System.Security.Claims;

namespace SmartLeaf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CareTaskController : ControllerBase
    {
        private readonly ICareTaskService _service;

        public CareTaskController(ICareTaskService service) => _service = service;

        [Authorize]
        [HttpGet("plant/{plantId}")]
        public async Task<IActionResult> GetByPlant(int plantId)
        {
            var tasks = await _service.GetByPlantIdAsync(plantId);
            return Ok(tasks);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCareTaskRequest request)
        {
            var task = await _service.CreateAsync(request);
            return Ok(task);
        }

        [Authorize]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            await _service.UpdateStatusAsync(id, status);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
