namespace SmartLeaf.Domain
{
    public class CareTask
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public string TaskType { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
