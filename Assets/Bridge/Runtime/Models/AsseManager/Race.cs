using Bridge.Models.Common;

namespace Bridge.Models.AsseManager
{
    public class Race: IStageable, IEntity, INamed
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ReadinessId { get; set; }
        public long IntellectualPropertyId { get; set; }
        public string Prefab { get; set; }
    }
}