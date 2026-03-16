using System.Text.Json.Serialization;

public class ChatbotRequest
{
    [JsonPropertyName("contents")]
    public List<Content> Contents { get; set; } = new();

    public class Content
    {
        [JsonPropertyName("parts")]
        public List<Part> Parts { get; set; } = new();

        public class Part
        {
            [JsonPropertyName("text")]
            public string Text { get; set; } = string.Empty;
        }
    }
}