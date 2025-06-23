using System.Collections.Generic;
using Bridge.Models.Common;

namespace Bridge.Models.ClientServer.StartPack.Metadata
{
    public sealed class CharacterSpawnPositionFormationType: IEntity, INamed
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<CharacterSpawnPositionFormation> CharacterSpawnPositionFormations { get; set; }
    }
}