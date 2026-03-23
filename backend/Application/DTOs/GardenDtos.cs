namespace SmartLeaf.Application.DTOs
{
    public class CreateGardenRequest
    {
        public required string Name { get; set; }
        public int ClimateZoneId { get; set; }
        public int SoilTypeId { get; set; }
        public required string SunExposure { get; set; }
    }

    public class GardenResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ClimateZoneId { get; set; }
        public int SoilTypeId { get; set; }
        public string SunExposure { get; set; } = string.Empty;
    }

    public class UpdateGardenRequest
    {
        public string? Name { get; set; }
        public int? ClimateZoneId { get; set; }
        public int? SoilTypeId { get; set; }
        public string? SunExposure { get; set; }
    }
}
