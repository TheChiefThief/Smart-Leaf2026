using SmartLeaf.Application.DTOs;

namespace SmartLeaf.Application.Interfaces
{
    public interface IChatbotService
    {
        Task<ChatbotResponse> AskAsync(ChatbotRequest request);
    }
}
