namespace SmartLeaf.Domain
{
    public class SpeciesCatalog
    {
        public int Id { get; set; }
        public string ScientificName { get; set; } = string.Empty;
        public string CommonName { get; set; } = string.Empty;
        public string Family { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string BasicCare { get; set; } = string.Empty;
    }
}
