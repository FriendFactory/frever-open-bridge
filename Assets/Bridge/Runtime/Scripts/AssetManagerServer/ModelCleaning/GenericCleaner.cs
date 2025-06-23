using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.AssetManagerServer.ModelCleaning
{
    internal abstract class GenericCleaner<TEntity> : SendModelCleaner where TEntity: class, IEntity
    {
        private const string ID_PROP_NAME = nameof(IEntity.Id);
        private const string FILES_PROPERTY = nameof(IFilesAttachedEntity.Files);
        private static Type[] CLEAN_IGNORE_TYPES => new[] {typeof(FileInfo)};
        public sealed override Type TargetType { get; } = typeof(TEntity);
        protected abstract Type[] AllowedTypesToCreate { get; }

        public sealed override T Clean<T>(T model, bool cleanFromSyncedFilesData)
        {
            if (TargetType != model.GetType())
                throw new Exception($"You can use this cleaner of {TargetType.Name} for entity type {typeof(T).Name}");
            
            var clone = Clone(model);
          
            CleanupRecursively(ref clone, cleanFromSyncedFilesData);
            return clone;
        }

        private void CleanupRecursively<T>(ref T model, bool cleanFromSyncedFilesData)
        {
            var targetType = model.GetType();
            
            var foreignKeys = targetType.GetProperties()
                .Where(x => x.Name.EndsWith(ID_PROP_NAME) && x.Name.Length > ID_PROP_NAME.Length);

            foreach (var foreignKey in foreignKeys)
            {
                var navPropName = foreignKey.Name.Substring(0, foreignKey.Name.Length - 2);
                var navProperty = targetType.GetProperty(navPropName);
                if (navProperty == null)
                    continue;
                CleanupNavigationProperty(foreignKey, navProperty, model, cleanFromSyncedFilesData);
            }

            var collectionType = typeof(IEnumerable);
            var arraysProperty = targetType.GetProperties().Where(x => collectionType.IsAssignableFrom(x.PropertyType)).ToArray();
            foreach (var propertyInfo in arraysProperty)
            {
                var propValue = propertyInfo.GetValue(model);
                if(propValue == null)
                    continue;

                if (!(propValue is IEnumerable collection)) 
                    continue;

                var genericType = collection.GetType().GenericTypeArguments.FirstOrDefault();
                if (genericType != null && CLEAN_IGNORE_TYPES.All(x=>x!=genericType) 
                                        && AllowedTypesToCreate.All(x => x != genericType))
                {
                    propertyInfo.SetValue(model, null);
                    continue;
                }
                
                if (cleanFromSyncedFilesData && FILES_PROPERTY == propertyInfo.Name)
                {
                    var val = propValue as List<FileInfo>;
                    CleanFromSyncedWithServerTypes(ref val);
                    propertyInfo.SetValue(model, val);
                    continue;
                }
                
                var castCollection = collection.Cast<object>().ToArray();
                for (int i = 0; i < castCollection.Length; i++)
                {
                    CleanupRecursively(ref castCollection[i], cleanFromSyncedFilesData);
                }
            }

        }

        private void CleanupNavigationProperty<T>(PropertyInfo fK, PropertyInfo navProperty, T model, bool cleanFromSyncedFilesData)
        {
            var propValue = navProperty.GetValue(model);
            if(propValue==null)
                return;

            if(CLEAN_IGNORE_TYPES.Any(x => x.Name == navProperty.Name))
                return;

            if (AllowedTypesToCreate.Any(x => x == navProperty.PropertyType))
            {
                CleanupRecursively(ref propValue, cleanFromSyncedFilesData);
                return;
            }
            
            var id = propValue.GetType().GetProperty(ID_PROP_NAME);
            fK.SetValue(model,id.GetValue(propValue));
            navProperty.SetValue(model, null);
        }

        private void CleanFromSyncedWithServerTypes(ref List<FileInfo> fileInfos)
        {
            if(fileInfos==null)
                return;

            fileInfos = fileInfos.Where(x => x.State != FileState.SyncedWithServer).ToList();
        }
    }
}