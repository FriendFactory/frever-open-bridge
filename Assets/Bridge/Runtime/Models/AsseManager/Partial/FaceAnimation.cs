using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class FaceAnimation: IAsset, IMainFileContainable, ITaggable
    {
        public List<FileInfo> Files { get; set; }
    };
}
