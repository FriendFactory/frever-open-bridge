using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Bridge;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;
using Bridge.Modules.Serialization;
using Bridge.Services.AssetService.Caching;
using Bridge.Services.AssetService.Caching.AssetReaders;
using Bridge.Services.AssetService.Caching.Encryption;
using NUnit.Framework;
using FileInfo = Bridge.Models.Common.Files.FileInfo;

namespace Tests.MainServer.CacheTests
{
    public class CacheCleanTest 
    {
        [Test]
        public async void ClearCache_ShouldRemoveAllFiles()
        {
            var encryptionService = new EncryptionService();
            var cache = new AssetsCache("Cache", FFEnvironment.Develop, new AssetReaderProvider(encryptionService),  new Serializer(), encryptionService);

            var model = new SetLocationBundle();
            model.Id = 1;
            model.Files = new List<FileInfo>();
            var fileInfo = new FileInfo(FileType.MainFile, Guid.NewGuid().ToString())
            {
                Extension = FileExtension.Empty
            };
            model.Files.Add(fileInfo);

            var data = Encoding.ASCII.GetBytes("Just test text");
            var stream = new MemoryStream(data);

            await cache.SaveToCacheAsync(model, fileInfo, stream, CancellationToken.None);

            var isFileInCache = cache.HasInCache(model, fileInfo);
            Assert.IsTrue(isFileInCache);

            cache.Clear();
            isFileInCache = cache.HasInCache(model, fileInfo);
            Assert.IsFalse(isFileInCache);

            var cacheFilePathGetter = cache.GetType().GetMethod("GetCacheLinksFilePath", BindingFlags.NonPublic| BindingFlags.Instance);
            var cacheFilePath = (string)cacheFilePathGetter.Invoke(cache, new Object[]{FFEnvironment.Develop});
            Assert.IsFalse(File.Exists(cacheFilePath));
        }

        [Test]
        public void ClearEmptyCache_ShouldNotGetException()
        {
            var encryptionService = new EncryptionService();
            var cache = new AssetsCache("Cache",FFEnvironment.Develop, new AssetReaderProvider(encryptionService), new Serializer(), encryptionService);
            
            var res = cache.Clear();
            Assert.IsTrue(res.IsSuccess);
           
            res = cache.Clear();
            Assert.IsTrue(res.IsSuccess);
        }
    }
}
