namespace SmartLeaf.Domain
{
    public class Plant
    {
        public int Id { get; set; }
        public int GardenId { get; set; }
        public int SpeciesId { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public int InitialQuantity { get; set; }
        public DateTime PlantingDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? EndDate { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}