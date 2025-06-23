using System;
using Bridge.Models.AsseManager;

namespace Bridge.AssetManagerServer.ModelCleaning
{
    internal class SetLocationCleaner: GenericCleaner<SetLocation>
    {
        protected override Type[] AllowedTypesToCreate { get; } = new[]
            {typeof(VfxPositionGroup), typeof(VfxPositionGroupAndVfxSpawnPosition), typeof(SetLocationAndCharacterSpawnPosition)};
    }
}