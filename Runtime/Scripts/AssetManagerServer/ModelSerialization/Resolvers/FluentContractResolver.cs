using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bridge.AssetManagerServer.ModelCleaning;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bridge.AssetManagerServer.ModelSerialization.Resolvers
{
    internal sealed class FluentContractResolver : IgnoreFieldsContractorResolver
    {
        private readonly List<ISerializationSettings> _settings;

        public FluentContractResolver(List<ISerializationSettings> settings)
        {
            _settings = settings;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            if(_settings==null)
                throw new Exception("Fluent contract resolver must have set settings");
            
            var contract = base.CreateProperty(member, memberSerialization);

            var settings = _settings.Where(x =>
            {
                var memberInfo = x.GetType();
                return memberInfo.GenericTypeArguments[0] == (member as PropertyInfo)?.PropertyType;
            }).ToArray();
          
            if (!settings.Any())
                return contract;
            var rule = settings.SelectMany(x => x.Rules.OfType<PropertyRule>()).FirstOrDefault();
            rule?.Update(contract);
            return contract;
        }
    }
}