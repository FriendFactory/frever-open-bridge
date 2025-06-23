using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bridge.AssetManagerServer.ModelSerialization.Resolvers
{
    internal class SerializationSettings<T> : ISerializationSettings
    {
        private List<Rule> _rules { get; } = new List<Rule>();

        public IEnumerable<Rule> Rules { get; }

        public SerializationSettings()
        {
            Rules = _rules.AsEnumerable();
        }

        public PropertyRule<T, TProp> RuleFor<TProp>(Expression<Func<T, TProp>> prop)
        {
            var rule = new PropertyRule<T, TProp>(prop);
            _rules.Add(rule);
            return rule;
        }
    }
}