public class ChatbotResponse
{
    public List<Candidate> Candidates { get; set; } = new();
    public UsageMetadata UsageMetadata { get; set; } = new();
}

public class Candidate
{
    public Content Content { get; set; } = new();
    public string FinishReason { get; set; } = string.Empty;
    public double AvgLogprobs { get; set; }
}

public class Content
{
    public List<Part> Parts { get; set; } = new();
}

public class Part
{
    public string Text { get; set; } = string.Empty;
}

public class UsageMetadata
{
    public int PromptTokenCount { get; set; }
    public int CandidatesTokenCount { get; set; }
    public int TotalTokenCount { get; set; }
}
