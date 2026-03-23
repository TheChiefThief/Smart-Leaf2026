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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _service.GetTaskByIdAsync(id);
            if (task == null) return NotFound(new { message = "Tarea no encontrada." });
            return Ok(task);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCareTaskRequest request)
        {
            var task = await _service.CreateAsync(request);
            return Ok(task);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCareTaskRequest request)
        {
            await _service.UpdateTaskAsync(id, request);
            return NoContent();
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
