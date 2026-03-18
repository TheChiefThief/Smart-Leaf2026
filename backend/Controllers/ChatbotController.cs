using Microsoft.AspNetCore.Mvc;
using SmartLeaf.Application.DTOs;
using SmartLeaf.Application.Interfaces;

namespace SmartLeaf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotController(IChatbotService chatbotService) => _chatbotService = chatbotService;

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatbotRequest request)
        {
            try
            {
                var response = await _chatbotService.AskAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ChatbotController: {ex.Message}");
                return StatusCode(500, new { message = "Error al procesar la solicitud." });
            }
        }
    }
}