using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using JetBrains.Annotations;

namespace Bridge.Models.ClientServer
{
    [UsedImplicitly]
    public class GroupShortInfo
    {
        public long Id { get; set; }
        public string Nickname { get; set; }
        public long? MainCharacterId { get; set; }
        public List<FileInfo> MainCharacterFiles { get; set; }
    }
}