using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Assets
{
    public class SetLocationBackground: IFilesAttachedEntity, INamed, IBackgroundOption
    {
        public long Id { get; set; }
        public string Name { get; set; }
        [ProtoNewField(1)] public int? SortOrder { get; set; }
        public BackgroundOptionType Type => BackgroundOptionType.Image;
        public List<FileInfo> Files { get; set; }
    }
}