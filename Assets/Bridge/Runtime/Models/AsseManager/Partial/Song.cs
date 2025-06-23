using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class Song: IStageableAsset, IThumbnailOwner, INamed, IMainFileContainable, ITaggable, IPlayableMusic, ISortOrderable
    {
        public List<FileInfo> Files { get; set; }
        public void AttachTo(MusicController controller)
        {
            controller.SongId = Id;
            controller.Song = this;
        }
    }
}