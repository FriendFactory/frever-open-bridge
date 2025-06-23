using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class VideoClip : IEntity, IStageable, IMainFileContainable, IGroupAccessible
    {
        public List<FileInfo> Files { get; set; }
    }
}