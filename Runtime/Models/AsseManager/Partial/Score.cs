using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class Score: IStageableAsset, INamed, IFilesAttachedEntity
    {
        public List<FileInfo> Files { get; set; }
    }
}