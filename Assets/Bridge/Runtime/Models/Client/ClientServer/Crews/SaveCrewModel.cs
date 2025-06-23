using System.Collections.Generic;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Crews
{
    public sealed class SaveCrewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public List<FileInfo> Files { get; set; }
        public string MessageOfDay { get; set; }
        public long? LanguageId { get; set; }
    }
}