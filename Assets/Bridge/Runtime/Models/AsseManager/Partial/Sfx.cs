using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class Sfx: IStageableAsset, INamed, IMainFileContainable, ITaggable
    {
        public List<FileInfo> Files { get; set; }
    }
}