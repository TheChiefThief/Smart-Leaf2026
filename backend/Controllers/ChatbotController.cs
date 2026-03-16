using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly ChatbotService _chatbotService;

    public ChatbotController(ChatbotService chatbotService)
    {
        _chatbotService = chatbotService;
    }

[HttpPost("ask")]public async Task<IActionResult> AskChatbot([FromQuery] string accessToken, [FromBody] ChatbotRequest request)
{
    if (string.IsNullOrEmpty(accessToken))
    {
        return BadRequest(new { message = "El campo 'accessToken' es obligatorio." });
    }

    try
    {
        var response = await _chatbotService.AskChatbotAsync(accessToken, request);
        return Ok(response); // Devolver la respuesta completa al frontend
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error en el controlador: {ex.Message}");
        return StatusCode(500, new { message = "Error al procesar la solicitud." });
    }
}
}