using System;
using System.Collections.Generic;

namespace Bridge.AssetManagerServer.ModelSerialization.Resolvers
{
    internal abstract class SettingsSetup
    {
        public readonly Type TargetType;

        protected SettingsSetup(Type targetType)
        {
            TargetType = targetType;
        }

        public abstract List<ISerializationSettings> Get();
    }
}