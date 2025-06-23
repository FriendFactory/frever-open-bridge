using System.Collections.Generic;
using System.Linq;

namespace Bridge.AssetManagerServer.ModelSerialization.Resolvers
{
    internal abstract class Rule
    {
        private Dictionary<string, object> _rules { get; } = new Dictionary<string, object>();

        protected void AddRule(string key, object value)
        {
            if (_rules.ContainsKey(key))
            {
                _rules.Add(key, value);
            }
            else
            {
                _rules[key] = value;
            }
        }

        protected IEnumerable<KeyValuePair<string, object>> RegisteredRules => _rules.AsEnumerable();
    }
}