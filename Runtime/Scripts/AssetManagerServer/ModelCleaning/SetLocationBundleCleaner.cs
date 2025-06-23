using System;
using Bridge.Models.AsseManager;

namespace Bridge.AssetManagerServer.ModelCleaning
{
    internal sealed class SetLocationBundleCleaner: GenericCleaner<SetLocationBundle>
    {
        protected override Type[] AllowedTypesToCreate { get; } = Array.Empty<Type>();
    }
}