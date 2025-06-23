using System.Collections.Generic;

namespace Bridge.Models.AsseManager
{
    public partial class BodyAnimationGroup
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BodyAnimation> CharacterSpawnPosition { get; set; }
    }
}