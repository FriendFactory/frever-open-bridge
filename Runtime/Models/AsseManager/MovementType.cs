using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class MovementType
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool AutoSetOnAssetManager { get; set; }
        public int SortOrder { get; set; }

        public virtual ICollection<CharacterSpawnPosition> CharacterSpawnPosition { get; set; }
    }
}