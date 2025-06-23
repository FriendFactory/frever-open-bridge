using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public class UmaAdjustment: IEntity, INamed
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }
    }
}