namespace Bridge.Models.Common
{
    public sealed class LevelMetadataInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int XpRequired { get; set; }
        public int Level { get; set; }
        public int AssetLevelUnlocked { get; set; }
        public int AssetLevelVisible { get; set; }
    }
}