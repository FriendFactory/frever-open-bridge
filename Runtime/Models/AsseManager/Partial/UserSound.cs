using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class UserSound: IMainFileContainable, IPlayableMusic
    {
        public List<FileInfo> Files { get; set; }
        public void AttachTo(MusicController controller)
        {
            controller.UserSoundId = Id;
            controller.UserSound = this;
        }
    }
}