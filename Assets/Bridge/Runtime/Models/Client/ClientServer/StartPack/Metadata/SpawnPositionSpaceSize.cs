using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public sealed class SpawnPositionSpaceSize: IEntity, INamed
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int MaxFoV { get; set; }
        public int MaxDistance { get; set; }
        public int MinFoV { get; set; }
        public int MinDistance { get; set; }
        public int StartFoV { get; set; }
        public int StartDistance { get; set; }
    }
}