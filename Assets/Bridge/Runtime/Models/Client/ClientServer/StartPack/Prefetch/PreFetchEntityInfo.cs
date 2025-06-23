using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.StartPack.Prefetch
{
    public sealed class PreFetchEntityInfo: IFilesAttachedEntity
    {
        public long Id { get; set; }

        public List<FileInfo> Files { get; set; }
    }
}