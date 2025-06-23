using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class SetLocationBundle : IMainFileContainable, INamed, IStageableAsset, ISizeStorable
    {
        public List<FileInfo> Files { get; set; }
    }
}