namespace SmartLeaf.Domain
{
    public class Plant
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PlantTypeId { get; set; }
        public string CustomName { get; set; }
        public DateTime PlantedDate { get; set; }
        public string Region { get; set; }
        public int Status { get; set; }
    }
}