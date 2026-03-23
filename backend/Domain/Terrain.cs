using System.Text.Json;

namespace SmartLeaf.Domain
{
    public class Terrain
    {
        public int Id { get; set; }
        public int GardenId { get; set; }
        public string ShapeType { get; set; } = string.Empty; // e.g. "rectangle", "circle", "custom"
        public JsonDocument? Dimensions { get; set; }        // JSONB: { "width": 800, "height": 600 }
        public DateTime CreatedAt { get; set; }
    }
}
