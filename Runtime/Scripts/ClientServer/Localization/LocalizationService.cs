using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Modules.Serialization;
using UnityEngine;

namespace Bridge.ClientServer.Localization
{
    internal interface ILocalizationService
    {
        FFEnvironment Environment { get; }
        bool HasCached(string isoCode, out string path);
        
        Task<LocalizationDataResult> GetLocalizationData(string isoCode, CancellationToken token);
    }

    internal sealed class LocalizationService: ServiceBase ,ILocalizationService
    {
        private const string LOCALIZATION_CACHE_FOLDER = "Localization";
        private const string VERSION_HEADER = "x-localization-version";
        
        private CachedLocalizationFileData _cacheData;
        private readonly string _cacheRootFolder;

        public FFEnvironment Environment { get; }

        private string CacheMainFolder => Path.Combine(Application.persistentDataPath, $"{_cacheRootFolder}/{Environment}/{LOCALIZATION_CACHE_FOLDER}");
        private string CachedDataPath => $"{CacheMainFolder}/CachedData.json";
        
        
        public LocalizationService(string host, IRequestHelper requestHelper, ISerializer serializer, string cacheRootFolder, FFEnvironment ffEnvironment) : base(host, requestHelper, serializer)
        {
            _cacheRootFolder = cacheRootFolder;
            Environment = ffEnvironment;
            ReadCachedData();
        }

        public bool HasCached(string isoCode, out string path)
        {
            path = null;
            if (!_cacheData.IsoCodeToVersion.TryGetValue(isoCode, out var cachedVersion)) return false;
           
            path = GetLocalizationFilePath(isoCode, cachedVersion);
            return true;
        }

        public async Task<LocalizationDataResult> GetLocalizationData(string isoCode, CancellationToken token = default)
        {
            _cacheData.IsoCodeToVersion.TryGetValue(isoCode, out var cachedVersion);
            var lastCachedPath = string.IsNullOrEmpty(cachedVersion)
                ? string.Empty
                : GetLocalizationFilePath(isoCode, cachedVersion);
            try
            {
                var url = ConcatUrl(Host, $"localization/{isoCode}");

                var request = RequestHelper.CreateRequest(url, HTTPMethods.Get, false, false);
                request.DisableCache = true;
                request.AddHeader(VERSION_HEADER, cachedVersion ?? string.Empty);
                var resp = await request.GetHTTPResponseAsync(token);

                if (!resp.IsSuccess)
                {
                    return LocalizationDataResult.Failed(resp.Message, resp.StatusCode, lastCachedPath);
                }

                var actualVersion = resp.GetFirstHeaderValue(VERSION_HEADER);
                if (resp.StatusCode == 304 || actualVersion == cachedVersion)
                {
                    return LocalizationDataResult.Success(lastCachedPath);
                }

                var updatedFilePath = GetLocalizationFilePath(isoCode, actualVersion);
                if (!Directory.Exists(CacheMainFolder))
                {
                    Directory.CreateDirectory(CacheMainFolder);
                }
                
                await File.WriteAllTextAsync(updatedFilePath, resp.DataAsText, token);
                _cacheData.IsoCodeToVersion[isoCode] = actualVersion;
                SaveCachedDataInfo();
                if (!string.IsNullOrEmpty(lastCachedPath) && File.Exists(lastCachedPath))
                {
                    File.Delete(lastCachedPath);
                }

                return LocalizationDataResult.Success(updatedFilePath);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException? LocalizationDataResult.Cancelled() : LocalizationDataResult.Failed($"Frever exc: {e.Message}", -1, lastCachedPath);
            }
        }

        private void SaveCachedDataInfo()
        {
            File.WriteAllText(CachedDataPath, Serializer.SerializeToJson(_cacheData));
        }

        private void ReadCachedData()
        {
            if (!File.Exists(CachedDataPath))
            {
                _cacheData = new CachedLocalizationFileData();
                return;
            }

            var json = File.ReadAllText(CachedDataPath);
            try
            {
                _cacheData = Serializer.DeserializeJson<CachedLocalizationFileData>(json);
            }
            catch (Exception)
            {
                _cacheData = new CachedLocalizationFileData();
            }
        }

        private string GetLocalizationFilePath(string isoCode, string version)
        {
            return Path.Combine(CacheMainFolder, $"{isoCode}_{version}.csv");
        }
    }

    internal sealed class CachedLocalizationFileData
    {
        public Dictionary<string, string> IsoCodeToVersion = new();
    }
}