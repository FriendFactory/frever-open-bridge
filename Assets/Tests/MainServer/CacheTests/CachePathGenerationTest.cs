using System;
using System.Collections.Generic;
using Bridge;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;
using Bridge.Services.AssetService.Caching.CachePathGeneration;
using NUnit.Framework;

namespace Tests.MainServer.CacheTests
{
    public class CachePathGenerationTest 
    {
        [Test]
        public void GetWardrobeThumbnails128_ShouldReturnCorrectPath()
        {
            var expectedPath = $"Develop/Wardrobe/10/Thumbnail_128x128.png"; 
            
            var pathGenerator = new LocalFilesPathProvider(FFEnvironment.Develop);
            var wardrobe = new Wardrobe();
            wardrobe.Id = 10;
            wardrobe.Files = new List<FileInfo>();
            var fileInfo = new FileInfo(FileType.Thumbnail)
            {
                Extension = FileExtension.Png,
                Resolution = Resolution._128x128,
                Version = Guid.NewGuid().ToString()
            };
            
            wardrobe.Files.Add(fileInfo);
            var path = pathGenerator.GetPath(wardrobe, fileInfo);
            
            Assert.True(path == expectedPath);
        }
        
        [Test]
        public void GetUmaBundleMainFilePath_ShouldReturnCorrectPath()
        {
            var expectedPath = $"Develop/UmaBundle/10/iOS/AssetBundle"; 
            
            var pathGenerator = new LocalFilesPathProvider(FFEnvironment.Develop);
            var wardrobe = new UmaBundle();
            wardrobe.Files = new List<FileInfo>();

            var fileInfo = new FileInfo(FileType.MainFile)
            {
                Extension = FileExtension.Empty
            };
            wardrobe.Files.Add(fileInfo);
            wardrobe.Id = 10;
            var path = pathGenerator.GetPath(wardrobe, fileInfo);
            
            Assert.True(path == expectedPath);
        }
    }
}
