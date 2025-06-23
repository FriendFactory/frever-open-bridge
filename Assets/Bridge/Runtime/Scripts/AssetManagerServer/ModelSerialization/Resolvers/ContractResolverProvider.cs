using System.Collections.Generic;
using System.Linq;
using Bridge.AssetManagerServer.ModelCleaning;
using Bridge.Models.Common;

namespace Bridge.AssetManagerServer.ModelSerialization.Resolvers
{
    internal class ContractResolverProvider
    {
        private readonly List<SettingsSetup> _settings = new List<SettingsSetup>()
        {
            new WardrobeSettings()
        };
        
        public IgnoreFieldsContractorResolver GetResolver<T>() where T:IEntity
        {
            var targetType = typeof(T);
            var settings = _settings.FirstOrDefault(x => x.TargetType == targetType);
            if (settings!=null)
            {
               return new FluentContractResolver(settings.Get());
            }
            
            return new IgnoreFieldsContractorResolver();
        }
    }
}
