using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public class CameraFilterCategory: IAssetCategory
    {
        public long Id { get; set; } 
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public bool HasNew { get; set; }
    }
}