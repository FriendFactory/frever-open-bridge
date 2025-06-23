using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class VoiceFilterFullInfo: IThumbnailOwner, INamed
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<FileInfo> Files { get; set; }
        public int? Volume { get; set; }
        public int? Pitch { get; set; }
        public int? SendLevel { get; set; }
        public int? WetMixLevel { get; set; }
        public int? EffectParameters { get; set; }
        public long VoiceFilterCategoryId { get; set; }
        public long[] Tags { get; set; }
        public int SortOrder { get; set; }
        public string DisplayName { get; set; }
        public AssetOfferInfo AssetOffer { get; set; }
    }
}