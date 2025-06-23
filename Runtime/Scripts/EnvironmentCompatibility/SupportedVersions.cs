namespace Bridge.EnvironmentCompatibility
{
    public class SupportedVersions
    {
        public string BridgeMinVersion { get; set; }
        public string BridgeMaxVersion { get; set; }

        public int FreverMinBuild { get; set; }
        public int FreverMaxBuild { get; set; }

        public string FreverMinVersion { get; set; }
        public string FreverMaxVersion { get; set; }
    }
}