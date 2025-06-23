using System;
using System.Linq;
using Bridge.AssetManagerServer.ModelCleaning;
using Bridge.AssetManagerServer.ModelSerialization.Resolvers;
using Bridge.Models.Common;
using Bridge.Modules.Serialization;
using JsonDiffPatchDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bridge.AssetManagerServer
{
    public sealed class DifferenceDeepUpdateReq<T>: OptimizedUpdateReqBase<T> where T:IEntity
    {
        internal override T OriginModel => _origin;
        internal override T TargetModel => _modified;

        public override bool HasDataToUpdate
        {
            get
            {
                if(_hasDataToUpdate==null)
                    throw new InvalidOperationException($"Property {nameof(HasDataToUpdate)} should be invoked after {nameof(BuildQueryObject)}");
                return _hasDataToUpdate.Value;
            }
        }

        private readonly T _origin;
        private readonly T _modified;

        private readonly ContractResolverProvider _contractResolverProvider;
        private readonly ModelCleanerProvider _modelCleanerProvider;

        private JToken _comparingResult;
        private bool? _hasDataToUpdate;
        
        public DifferenceDeepUpdateReq(T origin, T modified)
        {
            _origin = origin;
            _modified = modified;
            
            _contractResolverProvider = new ContractResolverProvider();
            _modelCleanerProvider = new ModelCleanerProvider();
        }

        public void CompareObjects(bool compareFilesData = true)
        {
            BuildQueryObject(compareFilesData);
        }

        protected override object BuildQueryObjectInternal(bool includeFilesData)
        {
            if (_comparingResult != null)
                return _comparingResult;
            
            if(_origin.Id != _modified.Id)
                throw new Exception("Origin and modified entities has different Id. It should not be happen.");

            if(ReferenceEquals(_origin, _modified))
                throw new Exception("Origin and Modified objects are the same objects");

            var cleaner = _modelCleanerProvider.GetCleaner<T>();
            
            var originCleaned = cleaner.Clean(_origin, true);
            var modifiedCleaned = cleaner.Clean(_modified, true);

            var settings = new JsonSerializer()
            {
                ContractResolver = _contractResolverProvider.GetResolver<T>(),
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            };
            settings.Converters.Add(new Vector2Converter());
            settings.Converters.Add(new Vector3Converter());
            settings.Converters.Add(new QuaternionConverter());
            
            var originAsJson = JObject.FromObject(originCleaned, settings);
            var modifiedAsJson = JObject.FromObject(modifiedCleaned, settings);
            
            var jdp = new JsonDiffPatch {MainPropertyName = "id", IgnoreFilesData = !includeFilesData};
            _comparingResult = jdp.Diff(originAsJson, modifiedAsJson);
            _hasDataToUpdate = _comparingResult.Count() > 1 || ((JObject)_comparingResult).Properties().Count()>1;
            return _comparingResult;
        }
    }
}
