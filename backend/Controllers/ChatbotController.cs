using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLeaf.Application.DTOs;
using SmartLeaf.Application.Interfaces;

namespace SmartLeaf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotController(IChatbotService chatbotService) => _chatbotService = chatbotService;

        /// <summary>
        /// Envía una pregunta al chatbot de jardinería (Gemini).
        /// </summary>
        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatbotRequest request)
        {
            var response = await _chatbotService.AskAsync(request);
            return Ok(response);
        }
    }
}