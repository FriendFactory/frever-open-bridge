using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.ClientServer.ImageGeneration
{
    public sealed class GeneratedImage: IMainFileContainable
    {
        public long Id { get; set; }
        public List<FileInfo> Files { get; set; }
    }
}