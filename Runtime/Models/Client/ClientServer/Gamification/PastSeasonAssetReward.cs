using System.Collections.Generic;
using Bridge.Models.ClientServer.AssetStore;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Gamification
{
    public class PastSeasonAssetReward: IThumbnailOwner
    {
        public long Id { get; set; }

        public AssetInfo Asset { get; set; }

        public string Title { get; set; }

        public List<FileInfo> Files { get; set; }
    }
}