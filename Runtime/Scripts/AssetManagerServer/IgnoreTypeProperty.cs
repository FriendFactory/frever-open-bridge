using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bridge.AssetManagerServer
{
    internal class IgnoreTypeProperty: DefaultContractResolver
    {
        private const string IgnoredTypeProperty = "$type";

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.PropertyName == IgnoredTypeProperty)
            {
                property.ShouldSerialize = i => false;
                property.Ignored = true;
            }

            return property;
        }
    }
}