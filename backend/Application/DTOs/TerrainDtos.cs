using System.Text.Json;

namespace SmartLeaf.Application.DTOs
{
    // ─── Terrain ──────────────────────────────────────────────────────────────

    public class CreateTerrainRequest
    {
        public int GardenId { get; set; }
        public required string ShapeType { get; set; }  // "rectangle" | "circle" | "custom"
        public JsonDocument? Dimensions { get; set; }   // { "width": 800, "height": 600 }
    }

    public class UpdateTerrainRequest
    {
        public string? ShapeType { get; set; }
        public JsonDocument? Dimensions { get; set; }
    }

    public class TerrainResponse
    {
        public int Id { get; set; }
        public int GardenId { get; set; }
        public string ShapeType { get; set; } = string.Empty;
        public object? Dimensions { get; set; }   // Se serializa desde JsonDocument
        public DateTime CreatedAt { get; set; }
        public List<SproutResponse> Sprouts { get; set; } = [];
    }

    // ─── Sprout ───────────────────────────────────────────────────────────────

    public class CreateSproutRequest
    {
        public int TerrainId { get; set; }
        public int PlantId { get; set; }            // FK → plants (jardín del usuario)
        public string Label { get; set; } = string.Empty;
        public string Color { get; set; } = "#4CAF50";
        public string Form { get; set; } = "circle"; // "circle" | "square" | "leaf"
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }

    public class UpdateSproutRequest
    {
        public string? Label { get; set; }
        public string? Color { get; set; }
        public string? Form { get; set; }
        public double? X { get; set; }
        public double? Y { get; set; }
        public double? Z { get; set; }
    }

    public class SproutResponse
    {
        public int Id { get; set; }
        public int TerrainId { get; set; }
        public int PlantId { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Form { get; set; } = string.Empty;
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
