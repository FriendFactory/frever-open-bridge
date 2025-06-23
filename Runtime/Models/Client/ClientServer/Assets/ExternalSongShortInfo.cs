using Bridge.ExternalPackages.Protobuf;

namespace Bridge.Results
{
    public class ExternalSongShortInfo
    {
        public long Id { get; set; }
        public bool IsAvailable { get; set; }
        [ProtoNewField(1)] public int UsageCount { get; set; }
        [ProtoNewField(2)] public bool IsFavorite { get; set; }
    }
}