using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public sealed class WardrobeCategoryType: IEntity, INamed
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}