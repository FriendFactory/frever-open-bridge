using System;
using System.Collections.Generic;
using System.Reflection;
using Bridge.Models.Common;
using JsonDiffPatchDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bridge.AssetManagerServer.ModelCleaning
{
    internal class IgnoreFieldsContractorResolver : CamelCasePropertyNamesContractResolver
    {
        private readonly List<string> _ignoredProperties = new List<string>();
        private static readonly Type FilesOwnerType = typeof(IFilesAttachedEntity);
        
        public void IgnoreProperty(params string[] jsonPropertyNames)
        {
            if (jsonPropertyNames == null)
                return;

            foreach (var propertyName in jsonPropertyNames)
            {
                var camelCaseName = propertyName.FirstCharToLower();
                if (!_ignoredProperties.Contains(camelCaseName)) _ignoredProperties.Add(camelCaseName);
            }
        }

        public override JsonContract ResolveContract(Type type)
        {
            return FilesOwnerType.IsAssignableFrom(type) ? CreateContract(type) : base.ResolveContract(type);
        }
        
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (IsIgnored(property.PropertyName))
            {
                property.ShouldSerialize = i => false;
                property.Ignored = true;
            }

            return property;
        }

        private bool IsIgnored(string jsonPropertyName)
        {
            return _ignoredProperties.Contains(jsonPropertyName);
        }
    }
}