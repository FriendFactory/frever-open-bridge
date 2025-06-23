using System.Collections.Generic;
using System.Text.RegularExpressions;
using Bridge.AssetManagerServer.ModelCleaning;
using Bridge.AssetManagerServer.ModelSerialization.Converters;
using Bridge.AssetManagerServer.ModelSerialization.Resolvers;
using Bridge.Models.AsseManager;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using JsonDiffPatchDotNet;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;
using Resolution = Bridge.Models.Common.Files.Resolution;

namespace Tests.MainServer.SerializationTests
{
    public class IgnoreFieldsResolverTests 
    {
        [Test]
        public void IgnoreFilesField_ResultJsonShouldNotContainFilesField()
        {
            var model = new Song();
            model.Files = new List<FileInfo>();
            var thumbnail = new FileInfo(FileType.Thumbnail)
            {
                Source = new FileSource()
                {
                    UploadId = "Bla"
                }
            };
            model.Files.Add(thumbnail); 
            
            var resolver = new IgnoreFieldsContractorResolver();
            resolver.IgnoreProperty(nameof(IFilesAttachedEntity.Files));

            var settings = new JsonSerializerSettings {ContractResolver = resolver};

            var json = JsonConvert.SerializeObject(model, settings);
            var filesField = $"{nameof(IFilesAttachedEntity.Files).FirstCharToLower()}:";
            Assert.IsFalse(json.Contains(filesField));
        }
        
        [Test]
        public void IgnoreWardrobeFieldsInIncludedModels_ShouldIgnoreAllExceptInRootWardrobe()
        {
            var model = new Wardrobe {Id = 1, Name = "Wardrobe1",WardrobeGenderGroup = new WardrobeGenderGroup()};
            model.WardrobeGenderGroup.Wardrobe.Add(new Wardrobe(){Id = 1, Name = "Wardrobe1"});
            model.WardrobeGenderGroup.Wardrobe.Add(new Wardrobe(){Id = 2, Name = "Wardrobe2"});
            
            var sets = new List<ISerializationSettings>();
            var s1 = new SerializationSettings<WardrobeGenderGroup>();
            s1.RuleFor(x => x.Wardrobe).Converter(new WardrobeGenderGroupConverter());
            sets.Add(s1);
            
            var resolver = new FluentContractResolver(sets);

            resolver.IgnoreProperty(nameof(Wardrobe.Brand));
            
            var settings = new JsonSerializerSettings {ContractResolver = resolver};

            var json = JsonConvert.SerializeObject(model, settings);

            Debug.Log(json);
            Assert.IsTrue(Regex.Matches(json, "Wardrobe1").Count == 1);
            Assert.IsTrue(Regex.Matches(json, "Wardrobe2").Count == 0);
            Assert.IsTrue(Regex.Matches(json, nameof(Wardrobe.Brand)).Count == 0);
        }
        
        [Test]
        public void IgnoreWardrobeFieldsViaResolverProvider_ShouldIgnoreAllExceptInRootWardrobe()
        {
            var model = new Wardrobe {Id = 1, Name = "Wardrobe1",WardrobeGenderGroup = new WardrobeGenderGroup()};
            model.WardrobeGenderGroup.Wardrobe.Add(new Wardrobe(){Id = 1, Name = "Wardrobe1"});
            model.WardrobeGenderGroup.Wardrobe.Add(new Wardrobe(){Id = 2, Name = "Wardrobe2"});

            var resolver = new ContractResolverProvider().GetResolver<Wardrobe>();

            resolver.IgnoreProperty(nameof(Wardrobe.Brand));
            
            var settings = new JsonSerializerSettings {ContractResolver = resolver};

            var json = JsonConvert.SerializeObject(model, settings);

            Debug.Log(json);
            Assert.IsTrue(Regex.Matches(json, "Wardrobe1").Count == 1);
            Assert.IsTrue(Regex.Matches(json, "Wardrobe2").Count == 0);
            Assert.IsTrue(Regex.Matches(json, nameof(Wardrobe.Brand)).Count == 0);
        }

        [Test]
        public void CharacterSpawnPositionWithFileInfo_ShouldNotIgnoreFiles()
        {
            var spawnPos = new CharacterSpawnPosition();
            spawnPos.Files = new List<FileInfo>
            {
                new FileInfo("path", FileType.Thumbnail, Resolution._128x128),
                new FileInfo("path", FileType.Thumbnail, Resolution._256x256),
                new FileInfo("path", FileType.Thumbnail, Resolution._512x512)
            };

            var spawnCopy =
                JsonConvert.DeserializeObject<CharacterSpawnPosition>(JsonConvert.SerializeObject(spawnPos));
            
            var resolver = new IgnoreFieldsContractorResolver();
            resolver.IgnoreProperty(nameof(FileInfo.FilePath));
            resolver.IgnoreProperty(nameof(IFilesAttachedEntity.Files));

            var settings = new JsonSerializerSettings {ContractResolver = resolver};

            var json = JsonConvert.SerializeObject(spawnPos, settings);

            var result = JsonConvert.DeserializeObject<CharacterSpawnPosition>(json);
            Assert.IsTrue(result.Files==null);
            
            var resolver2 = new IgnoreFieldsContractorResolver();
            settings = new JsonSerializerSettings()
            {
                ContractResolver = resolver2
            };
            json = JsonConvert.SerializeObject(spawnCopy, settings);
            result = JsonConvert.DeserializeObject<CharacterSpawnPosition>(json);
            
            Assert.IsTrue(result.Files!=null);
            Assert.IsTrue(result.Files.Count==spawnCopy.Files.Count);
        }
    }
}
