using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP;
using Bridge.Models.Common.Files;

namespace Bridge
{
    internal static class Extensions
    {
        public static string FixUrlSlashes(this string url)
        {
            return url.Replace('\\', '/');
        }

        public static string AddApiVersion(this string url, string apiVersion)
        {
            return url.Replace(".com/", $".com/{apiVersion}/");
        }
        
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static void SetAsSyncedWithServer(this IEnumerable<FileInfo> fileInfos)
        {
            foreach (var fileInfo in fileInfos)
            {
                fileInfo.TagAsSyncedWithServer();
                fileInfo.ReleaseAssetRawDataFromRAM();
            }
        }
        
        public static void ResetFilePaths(this IEnumerable<FileInfo> fileInfos)
        {
            foreach (var fileInfo in fileInfos)
            {
                fileInfo.ResetFilePath();
            }
        }

        public static void AddJsonContent(this HTTPRequest request, string json)
        {
            request.AddHeader("Content-Type", "application/json");
            request.RawData = Encoding.UTF8.GetBytes(json);
        }
        
        public static void AddHeaders(this HTTPRequest request, IDictionary<string, string> headers)
        {
            if (headers == null) return;
            foreach (var keyValuePair in headers)
            {
                request.AddHeader(keyValuePair.Key, keyValuePair.Value);
            }
        }
        
        public static string CombineUrls(string uri1, string uri2)
        {
            uri1 = uri1.TrimEnd('/');
            uri2 = uri2.TrimStart('/');
            return $"{uri1}/{uri2}";
        }

        /// <summary>
        /// need to compare nullable enum not just by "==", because IL2CPP has an issue with comparing property enum? {get;set} vs field enum?.
        /// in case both of them null -> it say "false" during comparing
        /// </summary>
        public static bool Compare<T>(this T? o1, T? o2) where T:struct
        {
            return (!o1.HasValue && !o2.HasValue)
                   || (o1.HasValue && o2.HasValue &&
                       o1.Value.Equals(o2.Value));
        }
    }
}