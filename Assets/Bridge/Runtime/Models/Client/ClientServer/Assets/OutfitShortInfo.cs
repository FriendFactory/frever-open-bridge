using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class OutfitShortInfo: IThumbnailOwner, INamed
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public SaveOutfitMethod SaveMethod { get; set; }
        public List<FileInfo> Files { get; set; }
    }
}