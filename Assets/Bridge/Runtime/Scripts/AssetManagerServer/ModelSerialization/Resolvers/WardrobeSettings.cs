using System.Collections.Generic;
using Bridge.AssetManagerServer.ModelSerialization.Converters;
using Bridge.Models.AsseManager;

namespace Bridge.AssetManagerServer.ModelSerialization.Resolvers
{
    internal class WardrobeSettings: SettingsSetup
    {
        public WardrobeSettings() : base(typeof(Wardrobe))
        {
        }

        public override List<ISerializationSettings> Get()
        {
            var sets = new List<ISerializationSettings>();
            var s1 = new SerializationSettings<WardrobeGenderGroup>();
            s1.RuleFor(x => x.Wardrobe).Converter(new WardrobeGenderGroupConverter());
            sets.Add(s1);
            return sets;
        }
    }
}