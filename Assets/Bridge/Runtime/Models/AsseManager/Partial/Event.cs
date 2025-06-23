using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class Event : IEntity, IGroupAccessible, IThumbnailOwner, IOrderable
    {
        public List<FileInfo> Files { get; set; }
        public int OrderNumber => LevelSequence;
    }
}