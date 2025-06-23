using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using Newtonsoft.Json;
using UnityEngine;

namespace Bridge.AssetManagerServer.ModelDataSynchronization
{
    internal class ModelSynchronizer<T> where T: IEntity
    {
        private T Dest;
        private T Source;
        
        private const string ID_PROP_NAME = nameof(IEntity.Id);
        private const string VERSION_ID_NAME = nameof(FileInfo.Version);
        private const string EXTENSION_NAME = nameof(FileInfo.Extension);
        private static readonly Type FILE_INFO_TYPE = typeof(FileInfo);

        private static readonly Type BYTE_ARRAY_TYPE = typeof(IEnumerable<byte>);
        private static readonly Type ORDERABLE_TYPE = typeof(IOrderable);
        private static readonly Type STRING_TYPE = typeof(string);

        public ModelSynchronizer(T source, T dest)
        {
            Source = source;
            Dest = dest;
        }

        public void Sync()
        {
            SyncInternal(Source, Dest, null);
        }

        private void SyncInternal<TU>(TU source, TU dest, object parentRootObject)
        {
            var targetType = source.GetType();

            //syncId
            var idProperty = targetType.GetProperty(ID_PROP_NAME);
            if (idProperty != null && (long)idProperty.GetValue(dest) == 0)
            {
                idProperty.SetValue(dest, idProperty.GetValue(source));
            }

            SyncForeignKeys(source, dest, parentRootObject);
            SyncCollections(source, dest);
            
            if (targetType == FILE_INFO_TYPE)
            {
                SyncFileInfo(source, dest);
            }
        }

        private void SyncForeignKeys<TU>(TU source, TU dest, object parentRootObject)
        {
            var targetType = source.GetType();
            var allForeignKeys = targetType.GetProperties().Where(x => x.Name.EndsWith(ID_PROP_NAME) && x.Name != ID_PROP_NAME).ToArray();
            if (allForeignKeys.Any())
            {
                foreach (var foreignKey in allForeignKeys)
                {
                    var navigationPropertyName = foreignKey.Name.Replace(ID_PROP_NAME, string.Empty);
                    var propertyInfo = targetType.GetProperty(navigationPropertyName);
                    if (propertyInfo == null)
                    {
                        continue;
                    }
                    
                    //skip read properties
                    if(!propertyInfo.CanWrite)
                        continue;
                    
                    //sync foreign keys values
                    SyncForeignKeyValue(foreignKey, propertyInfo, source, dest);

                    var sourceNavigationValue = propertyInfo.GetValue(source);
                    var destNavigationValue = propertyInfo.GetValue(dest);

                    if (sourceNavigationValue != null && destNavigationValue != null)
                    {
                        //recursively sync id in referenced objects 
                        SyncInternal(sourceNavigationValue, destNavigationValue, dest);
                    }

                    //sync parent foreign key
                    if (parentRootObject != null)
                    {
                        var parentType = parentRootObject.GetType();
                        var navigationPropName = parentType.Name + ID_PROP_NAME;
                        var targetProperty = allForeignKeys.FirstOrDefault(x => x.Name == navigationPropName);
                        if (targetProperty != null)
                        {
                            var parentId = parentType.GetProperty(ID_PROP_NAME).GetValue(parentRootObject);
                            targetProperty.SetValue(dest, parentId);
                        }
                    }
                }
            }
        }
        
        private void SyncForeignKeyValue(PropertyInfo foreignKeyProp, PropertyInfo navProp, object source, object dest)
        {
            if (source == null || dest == null)
                return;

            var sourceValue = (long?)foreignKeyProp.GetValue(source);
            if (sourceValue.HasValue && sourceValue != 0)
            {
                foreignKeyProp.SetValue(dest, sourceValue);
                return;
            }

            var navValue = navProp.GetValue(source);
            if (navValue != null)
            {
                var idProp = navValue.GetType().GetProperty(ID_PROP_NAME);
                var idValue = idProp.GetValue(navValue);
                foreignKeyProp.SetValue(dest, idValue);
            }
        }

        private void SyncCollections<TU>(TU source, TU dest)
        {
            var targetType = source.GetType();
            
            var collectionType = typeof(IEnumerable);
            var arraysProperty = targetType.GetProperties().Where(x => 
                    collectionType.IsAssignableFrom(x.PropertyType))
                .Where(x => x.PropertyType.GetElementType() == null || x.PropertyType.GetElementType().IsValueType).ToArray();

            if (arraysProperty.Any())
            {
                foreach (var property in arraysProperty)
                {
                    if (!(property.GetValue(source) is IEnumerable value) || !property.CanWrite)
                        continue;

                    if(BYTE_ARRAY_TYPE.IsInstanceOfType(value) || STRING_TYPE.IsInstanceOfType(value))
                        continue;
                    
                    var sourceArray = value.Cast<object>().ToArray();

                    if (!(property.GetValue(dest) is IEnumerable enumerable))
                        continue;

                    var destArray = enumerable.Cast<object>().ToArray();

                    if (destArray.Length > sourceArray.Length)
                    {
                        Debug.LogWarning($"BRIDGE: SYNC MODELS ID: Mistmatch in arrays:{JsonConvert.SerializeObject(sourceArray)} vs {JsonConvert.SerializeObject(destArray)}");
                    }

                    var entityType = property.PropertyType.IsArray
                        ?property.PropertyType.GetElementType()
                        :property.PropertyType.GetGenericArguments().FirstOrDefault();
                    
                    if (ORDERABLE_TYPE.IsAssignableFrom(entityType))
                    {
                        sourceArray = sourceArray.Cast<IOrderable>().OrderBy(x => x.OrderNumber).Cast<object>().ToArray();
                        destArray = destArray.Cast<IOrderable>().OrderBy(x => x.OrderNumber).Cast<object>().ToArray();
                    }
                    
                    for (var i = 0; i < destArray.Count(); i++)
                    {
                        var d = destArray.ElementAt(i);
                        if (sourceArray.Length == i)
                            break;

                        var s = sourceArray.ElementAt(i);
                        SyncInternal(s, d, dest);
                    }
                }
            }
        }

        private void SyncFileInfo<TU>(TU source, TU dest)
        {
            var targetType = source.GetType();

            var versionIdProp = targetType.GetProperty(VERSION_ID_NAME);
            if (versionIdProp != null && !string.IsNullOrEmpty(versionIdProp.GetValue(source) as string))
            {
                versionIdProp.SetValue(dest, versionIdProp.GetValue(source));
            }
            
            var extensionProp = targetType.GetProperty(EXTENSION_NAME, BindingFlags.NonPublic | BindingFlags.Instance);
            if (extensionProp != null)
            {
                extensionProp.SetValue(dest, extensionProp.GetValue(source));
            }
        }
    }
}