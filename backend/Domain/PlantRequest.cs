namespace SmartLeaf.Domain
{
    public class PlantRequest
    {
        public int PlantTypeId { get; set; }
        public string CustomName { get; set; }
        public DateTime PlantedDate { get; set; }
        public string Region { get; set; }
        public int Status { get; set; }
    }
}