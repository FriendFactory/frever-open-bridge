using System;
using System.Linq;
using Bridge.Models.AsseManager.Extensions.FilesContainable;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Bridge.Modules.Serialization;
using Newtonsoft.Json;

namespace Bridge.AssetManagerServer.ModelCleaning
{
    /// <summary>
    /// Clean sending model to reduce size of sending json
    /// For example, if we send an Event attached to some SetLocation, we should not send full SetLocation model
    /// SetLocationId will enough for back end
    /// </summary>
    internal abstract class SendModelCleaner
    {
        public abstract Type TargetType { get; }

        public abstract T Clean<T>(T model, bool cleanFromSyncedFilesData) where T:IEntity;

        protected T Clone<T>(T model) where T: IEntity
        {
            var contractResolver = new IgnoreFieldsContractorResolver();
            contractResolver.IgnoreProperty(nameof(FileInfo.FileRawData));

            var serializationSettings = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver
            };
            serializationSettings.Converters.Add(new Vector2Converter());
            serializationSettings.Converters.Add(new Vector3Converter());
            serializationSettings.Converters.Add(new QuaternionConverter());
            
            var json = JsonConvert.SerializeObject(model, serializationSettings);
            var clone = JsonConvert.DeserializeObject<T>(json);

            CopyReferencesForFileBytes(model, clone);
            return clone;
        }

        private void CopyReferencesForFileBytes<T>(T source, T dest) where T: IEntity
        {
            var sourceFileInfo = ExtractAllFileInfo(source);
            var destinationFileInfo = ExtractAllFileInfo(dest);

            for (var i = 0; i < sourceFileInfo.Length; i++)
            {
                var s = sourceFileInfo[i];
                if(s.FileRawData==null)
                    continue;

                var d = destinationFileInfo[i];
                d.FileRawData = s.FileRawData;
            }
        }

        private FileInfo[] ExtractAllFileInfo(IEntity entity)
        {
            return entity.ExtractAllModelWithFiles().Where(x => x.Files != null && x.Files.Count > 0)
                .SelectMany(x => x.Files).ToArray();
        }
    }
}