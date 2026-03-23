namespace SmartLeaf.Application.DTOs
{
    public class CreateCareTaskRequest
    {
        public int PlantId { get; set; }
        public required string TaskType { get; set; }
        public DateTime ScheduledDate { get; set; }
        public required string Status { get; set; }
    }

    public class CareTaskResponse
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public string TaskType { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class UpdateCareTaskRequest
    {
        public string? TaskType { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string? Status { get; set; }
    }
}
