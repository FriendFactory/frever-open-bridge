using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AsseManager.Extensions.FilesContainable
{
    /// <summary>
    /// This extension is responsible for extraction of all models, which has FileInfo list(in other words, represented by some files)
    /// </summary>
    public static class ExtractFileContainablesExtension 
    {
        /// <summary>
        /// It finds and returns all file containable models recursively in target entity 
        /// </summary>
        public static List<IFilesAttachedEntity> ExtractAllModelWithFiles<T>(this T model) where T: IEntity
        {
            var output = new List<IFilesAttachedEntity>();
            CollectFileModelsRecursively(model, output);
            return output;
        }

        private static void CollectFileModelsRecursively(object model, List<IFilesAttachedEntity> collection)
        {
            if (model is IFilesAttachedEntity fileContainer && fileContainer.Files != null)
            {
                collection.Add(fileContainer);
            }

            CollectFileModelsFromChildren(model, collection);
        }

        private static void CollectFileModelsFromChildren(object model, List<IFilesAttachedEntity> collection)
        {
            if (model is IEnumerable enumerable)
            {
                foreach (var o in enumerable)
                {
                    CollectFileModelsRecursively(o, collection);
                }
            }
            else
            {
                var children = GetChildren(model);

                foreach (var child in children)
                {
                    var val = child.GetValue(model);
                    if (val == null)
                        continue;
                    CollectFileModelsRecursively(val, collection);
                }
            }
        }

        private static IEnumerable<PropertyInfo> GetChildren(object obj)
        {
            return obj.GetType().GetProperties().Where(x =>
                !x.PropertyType.IsPrimitive
                && x.PropertyType != typeof(DateTime)
                && x.PropertyType != typeof(string)
                && x.PropertyType != typeof(List<FileInfo>)
                && x.PropertyType != typeof(UnityEngine.Vector2)
                && x.PropertyType != typeof(UnityEngine.Vector3)
                && x.PropertyType != typeof(UnityEngine.Quaternion));
        }
    }
}
