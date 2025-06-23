namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public class FeatureFlag
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string[] AvailableValues { get; set; }
    }
}