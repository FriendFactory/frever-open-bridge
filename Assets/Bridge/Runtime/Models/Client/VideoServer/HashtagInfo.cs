using Bridge.Models.Common;

namespace Bridge.Models.VideoServer
{
    public sealed class HashtagInfo: IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ViewsCount { get; set; }
        public long UsageCount { get; set; }
    }
}