using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager
{
    public partial class CharacterSpawnPosition: IThumbnailOwner, INamed, IUnityGuid, ISetLocationBundleChild, ITaggable
    {
        public List<FileInfo> Files { get; set; }
    }
}