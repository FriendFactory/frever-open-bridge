using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bridge.ExternalPackages.AsynAwaitUtility;
using Bridge.Modules.Serialization;
using Bridge.Services.Advertising;
using UnityEngine;
using UnityEngine.Networking;

namespace Bridge.Services.AssetService.Caching
{
    internal sealed class BannersCache
    {
        private readonly FFEnvironment _environment;
        private readonly List<SongAdData> _cachedFiles = new List<SongAdData>();
        private readonly ISerializer _serializer;
        private readonly string _persistentDataPath = Application.persistentDataPath;
        
        private string MainFolder =>Path.Combine(_persistentDataPath, $"Cache/{_environment.ToString()}");
        private string CachedDataPath => Path.Combine(MainFolder, "BannersCacheData");
        private string BannersFolder => Path.Combine(MainFolder, "Banners/PromotedSongs");
        
        public BannersCache(FFEnvironment environment, ISerializer serializer)
        {
            _environment = environment;
            _serializer = serializer;
        }

        public void LoadCache()
        {
            if(!File.Exists(CachedDataPath)) return;

            var json = File.ReadAllText(CachedDataPath);
            var cachedBanners = _serializer.DeserializeJson<List<SongAdData>>(json);
            if (cachedBanners != null)
            {
                _cachedFiles.AddRange(cachedBanners);   
            }
        }

        public bool HasCached(SongAdData songData)
        {
            return _cachedFiles.Any(songData.Equals);
        }

        public async Task<Texture2D> Get(SongAdData songData)
        {
            if (!HasCached(songData))
            {
                throw new InvalidOperationException(
                    "Failed attempt to get banner from the cache, when it is not cached");
            }

            var path = GetFilePath(songData);
            using var req = UnityWebRequestTexture.GetTexture($"file://{path}", true);
            await req.SendWebRequest();

            if (req.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                throw new InvalidOperationException(req.error);
            }
            return DownloadHandlerTexture.GetContent(req);
        }

        public async Task Cache(SongAdData songData, Texture2D texture)
        {
            var filePath = GetFilePath(songData);
            var folder = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            using (var stream = File.OpenWrite(filePath))
            {
                var bytes = texture.EncodeToPNG();
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
            _cachedFiles.Add(songData);
        }

        public void SaveMetadata()
        {
            var path = CachedDataPath;
            var folder = Path.GetDirectoryName(CachedDataPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var json = _serializer.SerializeToJson(_cachedFiles);
            File.WriteAllText(path, json);
        }

        public void Clear()
        {
            if (Directory.Exists(BannersFolder))
            {
                var dir = new DirectoryInfo(BannersFolder);
                dir.Delete(true);
            }
            _cachedFiles.Clear();

            if (File.Exists(CachedDataPath))
            {
                File.Delete(CachedDataPath);
            }
        }

        private string GetFilePath(SongAdData songData)
        {
            return Path.Combine(BannersFolder, songData.SongId.ToString());
        }
    }
}