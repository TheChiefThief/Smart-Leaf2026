namespace SmartLeaf.Domain
{
    public class Sprout
    {
        public int Id { get; set; }
        public int PlantId { get; set; }   // FK → plants
        public int TerrainId { get; set; } // FK → terrain
        public string Label { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Form { get; set; } = string.Empty;  // e.g. "circle", "square", "leaf"
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }     // opcional: orden de capas / altura
    }
}
