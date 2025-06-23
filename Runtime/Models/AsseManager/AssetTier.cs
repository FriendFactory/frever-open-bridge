using Bridge.Models.Common;

namespace Bridge.Models.AsseManager
{
    public partial class AssetTier: INamed, IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}