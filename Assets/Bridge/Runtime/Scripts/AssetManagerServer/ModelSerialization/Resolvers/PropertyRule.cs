using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Bridge.AssetManagerServer.ModelSerialization.Resolvers
{
    internal abstract class PropertyRule : Rule
    {
        public MemberInfo PropertyInfo { get; protected set; }

        public void Update(JsonProperty contract)
        {
            var props = typeof(JsonProperty).GetProperties();
            foreach (var rule in RegisteredRules)
            {
                var property = props.FirstOrDefault(x => x.Name == rule.Key);
                if (property != null)
                {
                    var value = rule.Value;
                    if (property.PropertyType.IsInstanceOfType(value))
                    {
                        property.SetValue(contract, value);
                    }
                }
            }
        }
    }
}