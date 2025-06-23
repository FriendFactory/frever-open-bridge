using System.Collections.Generic;

namespace Bridge.AssetManagerServer.ModelSerialization.Resolvers
{
    internal interface ISerializationSettings
    {
        IEnumerable<Rule> Rules { get; }
    }
}