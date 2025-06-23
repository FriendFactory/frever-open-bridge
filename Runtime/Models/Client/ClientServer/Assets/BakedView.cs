using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public sealed class BakedView: IMainFileContainable
    {
        public long Id { get; set; }
        public long? OutfitId { get; set; }
        public float HeelsHeight { get; set; }
        public List<FileInfo> Files { get; set; }
    }
}