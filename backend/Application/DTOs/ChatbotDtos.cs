using System.Text.Json.Serialization;

namespace SmartLeaf.Application.DTOs
{
    public class ChatbotRequest
    {
        [JsonPropertyName("contents")]
        public List<ChatbotContent> Contents { get; set; } = new();

        public class ChatbotContent
        {
            [JsonPropertyName("parts")]
            public List<ChatbotPart> Parts { get; set; } = new();

            public class ChatbotPart
            {
                [JsonPropertyName("text")]
                public string Text { get; set; } = string.Empty;
            }
        }
    }

    public class ChatbotResponse
    {
        public List<Candidate> Candidates { get; set; } = new();
        public UsageMetadata UsageMetadata { get; set; } = new();
    }

    public class Candidate
    {
        public ChatbotContent Content { get; set; } = new();
        public string FinishReason { get; set; } = string.Empty;
        public double AvgLogprobs { get; set; }
    }

    public class ChatbotContent
    {
        public List<ChatbotPart> Parts { get; set; } = new();
    }

    public class ChatbotPart
    {
        public string Text { get; set; } = string.Empty;
    }

    public class UsageMetadata
    {
        public int PromptTokenCount { get; set; }
        public int CandidatesTokenCount { get; set; }
        public int TotalTokenCount { get; set; }
    }
}
