using Bridge.Models.ClientServer.AssetStore;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Gamification
{
    public class SeasonAsset
    {
        public long Id { get; set; }

        public AssetStoreAssetType AssetType { get; set; }

        public string Title { get; set; }

        public FileInfo[] Files { get; set; }
    }
}