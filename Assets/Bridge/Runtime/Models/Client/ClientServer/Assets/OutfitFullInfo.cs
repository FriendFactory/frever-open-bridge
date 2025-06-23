using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class OutfitFullInfo: IThumbnailOwner, INamed
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public SaveOutfitMethod SaveMethod { get; set; }

        public Dictionary<long, List<long>> GenderWardrobes { get; set; }

        public List<FileInfo> Files { get; set; }

        public List<WardrobeFullInfo> Wardrobes { get; set; } = new List<WardrobeFullInfo>();

        public UmaSharedColorInfo[] UmaSharedColors { get; set; }
    }
}