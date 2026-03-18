namespace SmartLeaf.Domain
{
    public class SoilType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DrainageCapacity { get; set; } = string.Empty;
        public string OrganicLevel { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
