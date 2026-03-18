namespace SmartLeaf.Domain
{
    public class Garden
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int ClimateZoneId { get; set; }
        public int SoilTypeId { get; set; }
        public string SunExposure { get; set; } = string.Empty;
    }
}
