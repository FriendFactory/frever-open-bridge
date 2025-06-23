using System.Collections.Generic;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Recommendations
{
    public class RecommendationGroupInfo
    {
        public long Id { get; set; }
        public string Nickname { get; set; }
        public long? MainCharacterId { get; set; }
        public List<FileInfo> MainCharacterFiles { get; set; }
    }
}