namespace SmartLeaf.Domain
{
    public class ClimateZone
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Humidity { get; set; }
        public string TemperatureRange { get; set; } = string.Empty;
    }
}
