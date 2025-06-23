using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class OutfitSaveModel: IThumbnailOwner, INamed
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public SaveOutfitMethod SaveMethod { get; set; }
        
        public List<FileInfo> Files { get; set; }

        public List<long> WardrobeIds { get; set; } = new List<long>();

        public List<UmaSharedColorInfo> UmaSharedColors { get; set; } =
            new List<UmaSharedColorInfo>();
    }
}