using System;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Bridge.AssetManagerServer.ModelSerialization.Resolvers
{
    internal class PropertyRule<TClass, TProp> : PropertyRule
    {
        private const string CONVERTER_KEY = "Converter";
        private const string PROPERTY_NAME_KEY = "PropertyName";
        private const string IGNORED_KEY = "Ignored";

        public PropertyRule(Expression<Func<TClass, TProp>> prop)
        {
            PropertyInfo = (prop.Body as MemberExpression)?.Member;
        }

        public PropertyRule<TClass, TProp> Converter(JsonConverter converter)
        {
            AddRule(CONVERTER_KEY, converter);
            return this;
        }

        public PropertyRule<TClass, TProp> Name(string propertyName)
        {
            AddRule(PROPERTY_NAME_KEY, propertyName);
            return this;
        }

        public PropertyRule<TClass, TProp> Ignore()
        {
            AddRule(IGNORED_KEY, true);
            return this;
        }
    }
}