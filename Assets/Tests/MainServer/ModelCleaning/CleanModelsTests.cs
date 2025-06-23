using System.Collections.Generic;
using System.Linq;
using Bridge.AssetManagerServer.ModelCleaning;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;
using NUnit.Framework;

namespace Tests.MainServer.ModelCleaning
{
    public class CleanModelsTests 
    {
        private readonly ModelCleanerProvider _cleanerProvider = new ModelCleanerProvider();

        [Test]
        public void CleanLevelTeste_ShouldRemoveGroupAndSetForeignKey()
        {
            var level = new Level();
            level.Group = new Group()
            {
                Id = 10
            };

            var cleaner = _cleanerProvider.GetCleaner<Level>();
            var res = cleaner.Clean(level, false);

            Assert.AreEqual(null, res.Group);
            Assert.AreEqual(10, res.GroupId);
        }

        [Test]
        public void CleanLevelTest_ShouldRemoveCharactersAndSetForeignKeys()
        {
            var level = new Level();
            level.Event = new List<Event>();
            level.Event.Add(new Event()
            {
                CharacterController = new List<CharacterController>()
                {
                    new CharacterController()
                    {
                        Character = new Character()
                        {
                            Id = 1
                        }
                    },
                    new CharacterController()
                    {
                        Character = new Character()
                        {
                            Id = 2
                        }
                    },
                    new CharacterController()
                    {
                        Character = new Character()
                        {
                            Id = 3
                        }
                    }
                }
            });

            var cleaner = _cleanerProvider.GetCleaner<Level>();
            var res = cleaner.Clean(level, false);

            var characters = res.Event.SelectMany(x => x.CharacterController).Select(x => x.Character).ToArray();
            Assert.AreEqual(3, characters.Length);
            Assert.True(characters.All(x =>x==null));

            var characterForeignKeys = res.Event.SelectMany(x => x.CharacterController).Select(x => x.CharacterId).ToArray();
            Assert.AreEqual(3, characterForeignKeys.Length);
            for (int i = 1; i < 4; i++)
            {
                Assert.AreEqual(i,characterForeignKeys[i-1]);
            }
        }

        [Test]
        public void WardrobeCleaner_ShouldCleanUnityAssetTypeModel()
        {
            var wardrobe = new Wardrobe();
            wardrobe.UmaBundle = new UmaBundle();
            var umaAsset = new UmaAsset();
            wardrobe.UmaBundle.UmaAsset.Add(umaAsset);
            var umaAssetFile = new UmaAssetFile();
            umaAsset.UmaAssetFile.Add(umaAssetFile);

            umaAssetFile.UmaAssetFileAndUnityAssetType.Add(new UmaAssetFileAndUnityAssetType()
            {
                UnityAssetType = new UnityAssetType()
                {
                    Name = "Some Name"
                }
            });

            var cleaner = _cleanerProvider.GetCleaner<Wardrobe>();

            var cleaned = cleaner.Clean(wardrobe, false);
            
            Assert.IsTrue(cleaned.UmaBundle.UmaAsset.First().UmaAssetFile.First().UmaAssetFileAndUnityAssetType.First().UnityAssetType == null);
        }
       
        [Test]
        public void SetLocationBundleCleaner_ShouldRemoveKeepCharacterSpawnPosition()
        {
            var setLocationBundle = new SetLocationBundle();
            setLocationBundle.CharacterSpawnPosition.Add(new CharacterSpawnPosition()
            {
                Id = 1,
                Name = "Test"
            });

            var cleaner = _cleanerProvider.GetCleaner<SetLocationBundle>();
            var cleaned= cleaner.Clean(setLocationBundle, false);

            Assert.IsTrue(cleaned.CharacterSpawnPosition == null || cleaned.CharacterSpawnPosition.Count == 0);
        }

        [Test]
        public void GenericCleaner_CleansNonModifiedFiles()
        {
            var ev = new Event();
            ev.Id = 100;
            ev.Files = new List<FileInfo>();
            
            var modifiedFile = new FileInfo();
            modifiedFile.TagAsModified();
            ev.Files.Add(modifiedFile);
            
            var syncedWithServer = new FileInfo();
            syncedWithServer.TagAsSyncedWithServer();
            ev.Files.Add(syncedWithServer);

            var cleaner = _cleanerProvider.GetCleaner<Event>();
            var resultModel = cleaner.Clean(ev, true);
            
            Assert.True(resultModel.Files.Count() == 1);
            Assert.True(resultModel.Files.First().State == FileState.ModifiedLocally);
        }
    }
}
