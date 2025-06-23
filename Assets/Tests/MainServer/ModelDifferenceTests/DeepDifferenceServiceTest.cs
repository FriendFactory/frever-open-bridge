using System.Collections.Generic;
using System.Linq;
using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using JsonDiffPatchDotNet;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using Event = Bridge.Models.AsseManager.Event;
using Resolution = Bridge.Models.Common.Files.Resolution;

namespace Tests.MainServer.ModelDifferenceTests
{
    public class DeepDifferenceServiceTest
    {
        [Test]
        public void DeleteManyToManyItemsFromCollection_ShouldReturnNegativeIds()
        {
            var origin = new SetLocation();
            origin.Id = 10;
            origin.SetLocationAndCharacterSpawnPosition = new List<SetLocationAndCharacterSpawnPosition>();
            for (int i = 0; i < 4; i++)
            {
                origin.SetLocationAndCharacterSpawnPosition.Add(new SetLocationAndCharacterSpawnPosition()
                {
                    SetLocationId = origin.Id,
                    CharacterSpawnPositionId = i + 1
                });
            }

            var modified = JsonConvert.DeserializeObject<SetLocation>(JsonConvert.SerializeObject(origin));
            modified.SetLocationAndCharacterSpawnPosition.Remove(modified.SetLocationAndCharacterSpawnPosition.First());

            var deepQuery = new DifferenceDeepUpdateReq<SetLocation>(origin, modified);
            deepQuery.BuildQueryObject(true);
            var res = deepQuery.QueryObject;
            var resultSetLocation = JsonConvert.DeserializeObject<SetLocation>(res.ToString());
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition.Single(x=>x.CharacterSpawnPositionId == -1) != null);
        }

        [Test]
        public void AddNewManyToManyRelation_ShouldReturnItemsWithIdZero()
        {
            var origin = new SetLocation();
            origin.Id = 10;
            origin.SetLocationAndCharacterSpawnPosition = new List<SetLocationAndCharacterSpawnPosition>();

            var modified = JsonConvert.DeserializeObject<SetLocation>(JsonConvert.SerializeObject(origin));
            for (int i = 0; i < 4; i++)
            {
                modified.SetLocationAndCharacterSpawnPosition.Add(new SetLocationAndCharacterSpawnPosition()
                {
                    SetLocationId = modified.Id,
                    CharacterSpawnPositionId = i + 1
                });
            }

            var deepQuery = new DifferenceDeepUpdateReq<SetLocation>(origin, modified);
            deepQuery.BuildQueryObject(true);
            var res = deepQuery.QueryObject;
            Debug.Log(res);
            var resultSetLocation = JsonConvert.DeserializeObject<SetLocation>(res.ToString());
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition.Count == modified.SetLocationAndCharacterSpawnPosition.Count);
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition.All(x=>x.SetLocationId == 0));
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition.All(x => x.CharacterSpawnPositionId > 0));
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition
                .GroupBy(x => x.CharacterSpawnPositionId).All(x => x.Count() == 1));
        }

        [Test]
        public void AddNewManyToManyToExistedRelation_ShouldReturnNewItemsWithIdZeroButOldShouldStayWithNotZero()
        {
            var origin = new SetLocation();
            origin.Id = 10;
            origin.SetLocationAndCharacterSpawnPosition = new List<SetLocationAndCharacterSpawnPosition>();
            for (int i = 0; i < 2; i++)
            {
                origin.SetLocationAndCharacterSpawnPosition.Add(new SetLocationAndCharacterSpawnPosition()
                {
                    SetLocationId = origin.Id,
                    CharacterSpawnPositionId = i + 1
                });
            }

            var modified = JsonConvert.DeserializeObject<SetLocation>(JsonConvert.SerializeObject(origin));
            for (int i = 2; i < 4; i++)
            {
                modified.SetLocationAndCharacterSpawnPosition.Add(new SetLocationAndCharacterSpawnPosition()
                {
                    SetLocationId = 0,
                    CharacterSpawnPositionId = i + 1
                });
            }

            var deepQuery = new DifferenceDeepUpdateReq<SetLocation>(origin, modified);
            deepQuery.BuildQueryObject(true);
            var res = deepQuery.QueryObject;

            var resultSetLocation = JsonConvert.DeserializeObject<SetLocation>(res.ToString());
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition.Count == modified.SetLocationAndCharacterSpawnPosition.Count);
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition.Where(x=>x.CharacterSpawnPositionId>2).All(x => x.SetLocationId == 0));
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition.Where(x => x.CharacterSpawnPositionId <= 2).All(x => x.SetLocationId > 0));
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition.All(x => x.CharacterSpawnPositionId > 0));
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition
                .GroupBy(x => x.CharacterSpawnPositionId).All(x => x.Count() == 1));
        }

        [Test]
        public void AddNewSingleManyToManyToExistedRelation_ShouldReturnNewItemsWithIdZeroPaentId()
        {
            var origin = new SetLocation();
            origin.Id = 10;
            origin.SetLocationAndCharacterSpawnPosition = new List<SetLocationAndCharacterSpawnPosition>();

            var modified = JsonConvert.DeserializeObject<SetLocation>(JsonConvert.SerializeObject(origin));
            modified.SetLocationAndCharacterSpawnPosition.Add(new SetLocationAndCharacterSpawnPosition()
            {
                SetLocationId = modified.Id,
                CharacterSpawnPositionId = 1
            });

            var deepQuery = new DifferenceDeepUpdateReq<SetLocation>(origin, modified);
            deepQuery.BuildQueryObject(true);
            var res = deepQuery.QueryObject;

            var resultSetLocation = JsonConvert.DeserializeObject<SetLocation>(res.ToString());
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition.Count == modified.SetLocationAndCharacterSpawnPosition.Count);
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition.All(x => x.SetLocationId == 0));
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition.All(x => x.CharacterSpawnPositionId > 0));
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition
                .GroupBy(x => x.CharacterSpawnPositionId).All(x => x.Count() == 1));
        }

        [Test]
        public void TryCompareExactlyTheSameObjectsWithManyToMany_ShouldReturnEmptyManyToManyArray()
        {
            var origin = new SetLocation();
            origin.Id = 10;
            origin.SetLocationAndCharacterSpawnPosition = new List<SetLocationAndCharacterSpawnPosition>();
            for (int i = 0; i < 4; i++)
            {
                origin.SetLocationAndCharacterSpawnPosition.Add(new SetLocationAndCharacterSpawnPosition()
                {
                    SetLocationId = origin.Id,
                    CharacterSpawnPositionId = i + 1
                });
            }

            var copy = JsonConvert.DeserializeObject<SetLocation>(JsonConvert.SerializeObject(origin));

            var deepQuery = new DifferenceDeepUpdateReq<SetLocation>(origin, copy);
            deepQuery.BuildQueryObject(true);
            var res = deepQuery.QueryObject;

            var resultSetLocation = JsonConvert.DeserializeObject<SetLocation>(res.ToString());
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition.Count == 0);
        }

        [Test]
        public void CompareManyToManyWhenParentForeignKeyIsNullOrContainsParentId_ShouldReturnCorrectChanges()
        {
            var origin = new SetLocation();
            origin.Id = 10;
            origin.SetLocationAndCharacterSpawnPosition = new List<SetLocationAndCharacterSpawnPosition>();
            for (int i = 0; i < 2; i++)
            {
                origin.SetLocationAndCharacterSpawnPosition.Add(new SetLocationAndCharacterSpawnPosition()
                {
                    SetLocationId = origin.Id,
                    CharacterSpawnPositionId = i + 1
                });
            }

            var modified = JsonConvert.DeserializeObject<SetLocation>(JsonConvert.SerializeObject(origin));
            for (int i = 2; i < 3; i++)
            {
                modified.SetLocationAndCharacterSpawnPosition.Add(new SetLocationAndCharacterSpawnPosition()
                {
                    SetLocationId = 0,
                    CharacterSpawnPositionId = i + 1
                });
            }

            var deepQuery = new DifferenceDeepUpdateReq<SetLocation>(origin, modified);
            deepQuery.BuildQueryObject(true);
            var res = deepQuery.QueryObject;

            var resultSetLocation = JsonConvert.DeserializeObject<SetLocation>(res.ToString());
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition.Count == modified.SetLocationAndCharacterSpawnPosition.Count);
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition.Where(x => x.CharacterSpawnPositionId > 2).All(x => x.SetLocationId == 0));
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition.Where(x => x.CharacterSpawnPositionId < 2).All(x => x.SetLocationId > 0));
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition.All(x => x.CharacterSpawnPositionId > 0));
            Assert.IsTrue(resultSetLocation.SetLocationAndCharacterSpawnPosition
                .GroupBy(x => x.CharacterSpawnPositionId).All(x => x.Count() == 1));
        }

        [Test]
        public void CompareNotManyToManyArraysProperties()
        {
            var origin = new Level();
            origin.Event = new List<Event>();
            for (int i = 0; i < 5; i++)
            {
                origin.Event.Add(new Event()
                {
                    Id = i+1
                });
            }

            var modified = JsonConvert.DeserializeObject<Level>(JsonConvert.SerializeObject(origin));
            foreach (var e in modified.Event)
            {
                e.LevelSequence = 1;
            }

            var deepQuery = new DifferenceDeepUpdateReq<Level>(origin, modified);
            deepQuery.BuildQueryObject(true);
            var res = deepQuery.QueryObject;

            var resultLevel = JsonConvert.DeserializeObject<Level>(res.ToString());
            Debug.Log(res.ToString());
            Assert.True(resultLevel.Event.Count == origin.Event.Count);
            Assert.True(resultLevel.Event.All(x=>x.LevelSequence==1));
        }

        [Test]
        public void CompareEntityWithoutFilesAndWithFiles_ShouldKeepFilesModelInResult()
        {
            var origin = new Event();

            var modified = JsonConvert.DeserializeObject<Event>(JsonConvert.SerializeObject(origin));
            modified.Files = new List<FileInfo>();
            modified.Files.Add(new FileInfo("test/filepath.png", FileType.Thumbnail));

            var deepQuery = new DifferenceDeepUpdateReq<Event>(origin, modified);
            deepQuery.BuildQueryObject(true);
            var res = deepQuery.QueryObject;

            var resultEvent = JsonConvert.DeserializeObject<Event>(res.ToString());
            Debug.Log(res.ToString());
            Assert.True(resultEvent.Files!=null);
            Assert.True(resultEvent.Files.Any(x=>x.FileType == FileType.Thumbnail));
            Assert.True(resultEvent.Files.First(x=>x.FileType == FileType.Thumbnail).FilePath != null);
        }

        [Test]
        public void CompareEntityWithNotModifiedFiles_ShouldSkipFileModel()
        {
            var origin = new Event();
            origin.Files = new List<FileInfo>();
            var thumbnail = new FileInfo("test/filepath.png", FileType.Thumbnail);
            thumbnail.TagAsSyncedWithServer();
            origin.Files.Add(thumbnail);

            var modified = JsonConvert.DeserializeObject<Event>(JsonConvert.SerializeObject(origin));
          
            var deepQuery = new DifferenceDeepUpdateReq<Event>(origin, modified);
            deepQuery.BuildQueryObject(true);
            var res = deepQuery.QueryObject;

            var resultEvent = JsonConvert.DeserializeObject<Event>(res.ToString());
            Debug.Log(res.ToString());
            Assert.True(resultEvent.Files == null || resultEvent.Files.Count == 0);
            Assert.IsFalse(deepQuery.HasDataToUpdate);
        }

        [Test]
        public void CompareEntityWithNotModifiedFilesWithModified_ShouldKeepFullModel()
        {
            var origin = new Event();
            origin.Files = new List<FileInfo>();
            var thumbnail = new FileInfo("test/filepath.png", FileType.Thumbnail);
            origin.Files.Add(thumbnail);
            thumbnail.TagAsSyncedWithServer();

            var modified = JsonConvert.DeserializeObject<Event>(JsonConvert.SerializeObject(origin));
            modified.Files.First(x=>x.FileType == FileType.Thumbnail).TagAsModified();

            var deepQuery = new DifferenceDeepUpdateReq<Event>(origin, modified);
            deepQuery.BuildQueryObject(true);
            var res = deepQuery.QueryObject;

            var resultEvent = JsonConvert.DeserializeObject<Event>(res.ToString());
            Debug.Log(res.ToString());
            Assert.True(resultEvent.Files != null);
            thumbnail = resultEvent.Files.FirstOrDefault(x => x.FileType == FileType.Thumbnail);
            Assert.True(thumbnail != null);
            Assert.True(thumbnail.FilePath != null);
            Assert.True(thumbnail.State == FileState.ModifiedLocally);
            Assert.IsTrue(deepQuery.HasDataToUpdate);
        }

        [Test]
        public void CompareEntitiesListWithOnlySingleManyToMany_ShouldPassWithoutExceptions()
        {
            var setLocation = new SetLocation();
            setLocation.SetLocationAndCharacterSpawnPosition.Add(new SetLocationAndCharacterSpawnPosition()
            {
                CharacterSpawnPositionId = 20, SetLocationId = 10
            });

            var copy = JsonConvert.DeserializeObject<SetLocation>(JsonConvert.SerializeObject(setLocation));

            var diffRequest = new DifferenceDeepUpdateReq<SetLocation>(setLocation, copy);
            diffRequest.BuildQueryObject(true);
            Assert.IsFalse(diffRequest.HasDataToUpdate);
        }

        [Test]
        public void CompareEntityWithJoinArray_ShouldReturnCorrectJson()
        {
            var origin = new SetLocation
            {
                Id = 1,
                SetLocationAndCharacterSpawnPosition = new List<SetLocationAndCharacterSpawnPosition>
                {
                    new SetLocationAndCharacterSpawnPosition()
                    {
                        SetLocationId = 1, CharacterSpawnPositionId = 2
                    },
                    new SetLocationAndCharacterSpawnPosition() {SetLocationId = 1, CharacterSpawnPositionId = 3}
                }
            };


            var modified = JsonConvert.DeserializeObject<SetLocation>(JsonConvert.SerializeObject(origin));
            modified.SetLocationAndCharacterSpawnPosition.Remove(modified.SetLocationAndCharacterSpawnPosition.Last());
            
            var diffRequest = new DifferenceDeepUpdateReq<SetLocation>(origin, modified);
            diffRequest.BuildQueryObject(true);
            var json = diffRequest.QueryObject;
            var result = JsonConvert.DeserializeObject<SetLocation>(json.ToString());
            
            Assert.IsTrue(result.SetLocationAndCharacterSpawnPosition.Last().CharacterSpawnPositionId == -1 * origin.SetLocationAndCharacterSpawnPosition.Last().CharacterSpawnPositionId);
            Assert.IsTrue(diffRequest.HasDataToUpdate);
        }

        [Test]
        public void CharacterRecipeAndWardrobesUpdate()
        {
            var originCharacter = new Character() { Name = "TestCharacter", Id = 999 };

            // random J
            var umaRecipeJ = new byte[200];
            for (int i = 0; i < umaRecipeJ.Length; i++)
            {
                umaRecipeJ[i] = (byte)Random.Range(0, 256);
            }

            var umaRecipe = new UmaRecipe() { Id = 787, J = umaRecipeJ };

            umaRecipe.UmaRecipeAndWardrobe = new List<UmaRecipeAndWardrobe>()
            {
                new UmaRecipeAndWardrobe() { UmaRecipeId = umaRecipe.Id, WardrobeId = 123 },
                new UmaRecipeAndWardrobe() { UmaRecipeId = umaRecipe.Id, WardrobeId = 234 },
                new UmaRecipeAndWardrobe() { UmaRecipeId = umaRecipe.Id, WardrobeId = 345 },
                new UmaRecipeAndWardrobe() { UmaRecipeId = umaRecipe.Id, WardrobeId = 456 }
            };

            var characterAndUmaRecipe = new CharacterAndUmaRecipe(){ CharacterId = originCharacter.Id, UmaRecipe = umaRecipe, UmaRecipeId = umaRecipe.Id };

            originCharacter.CharacterAndUmaRecipe = new List<CharacterAndUmaRecipe>() { characterAndUmaRecipe };

            var modifiedCharacter = new Character() { Name = "TestCharacter", Id = 999 };

            // random modified J
            var umaRecipeJNew = new byte[190];
            for (int i = 0; i < umaRecipeJNew.Length; i++)
            {
                umaRecipeJNew[i] = (byte)Random.Range(0, 256);
            }
            var umaRecipeNew = new UmaRecipe() { Id = 787, J = umaRecipeJNew };
            umaRecipeNew.UmaRecipeAndWardrobe = new List<UmaRecipeAndWardrobe>()
            {
                new UmaRecipeAndWardrobe() { UmaRecipeId = umaRecipeNew.Id, WardrobeId = 123 },
                new UmaRecipeAndWardrobe() { UmaRecipeId = 0, WardrobeId = 5556 },
                new UmaRecipeAndWardrobe() { UmaRecipeId = umaRecipeNew.Id, WardrobeId = 456 }
            };

            var characterAndUmaRecipeNew = new CharacterAndUmaRecipe() { CharacterId = modifiedCharacter.Id, UmaRecipe = umaRecipeNew, UmaRecipeId = umaRecipeNew.Id };

            modifiedCharacter.CharacterAndUmaRecipe = new List<CharacterAndUmaRecipe>() { characterAndUmaRecipeNew };

            var diffRequest = new DifferenceDeepUpdateReq<Character>(originCharacter, modifiedCharacter);
            diffRequest.BuildQueryObject(true);
            var json = diffRequest.QueryObject;
            var diffChar = JsonConvert.DeserializeObject<Character>(json.ToString());
            var diffCharAndRecipe = diffChar.CharacterAndUmaRecipe.First();

            var encodedNewJ = System.Text.Encoding.ASCII.GetString(umaRecipeJNew);
            var encodedDiffJ = System.Text.Encoding.ASCII.GetString(diffCharAndRecipe.UmaRecipe.J);
            Assert.AreEqual(encodedNewJ, encodedDiffJ);

            var diffWardrobeList = diffCharAndRecipe.UmaRecipe.UmaRecipeAndWardrobe.ToList();

            var removed234 = diffWardrobeList.Find((w) => w.WardrobeId == -234);
            Assert.False(removed234 == null);

            var removed345 = diffWardrobeList.Find((w) => w.WardrobeId == -345);
            Assert.False(removed345 == null);

            var added5556 = diffWardrobeList.Find((w) => w.WardrobeId == 5556);
            Assert.False(added5556 == null);
            
            Assert.IsTrue(diffRequest.HasDataToUpdate);
        }

        [Test]
        public void UmaBundleDirectDependencies()
        {
            var umaBundle = new UmaBundle();
            umaBundle.Id = 10;
            umaBundle.UmaBundleAllDependencyDependsOnBundle.Add(new UmaBundleAllDependency()
            {
                UmaBundleId = 10,
                DependsOnBundleId = 20
            });
            umaBundle.UmaBundleAllDependencyDependsOnBundle.Add(new UmaBundleAllDependency()
            {
                UmaBundleId = 10,
                DependsOnBundleId = 30
            });

            var modified = JsonConvert.DeserializeObject<UmaBundle>(JsonConvert.SerializeObject(umaBundle));
            modified.UmaBundleAllDependencyDependsOnBundle.Clear();
            
            var request = new DifferenceDeepUpdateReq<UmaBundle>(umaBundle, modified);
            request.BuildQueryObject(true);
            var requestJson = request.QueryObject.ToString();
            var requestModel = JsonConvert.DeserializeObject<UmaBundle>(requestJson);
            
            Assert.IsTrue(request.HasDataToUpdate);
            Assert.True(requestModel.UmaBundleAllDependencyDependsOnBundle.ElementAt(0).DependsOnBundleId == -20);
            Assert.True(requestModel.UmaBundleAllDependencyDependsOnBundle.ElementAt(1).DependsOnBundleId == -30);
        }

        [Test]
        public void FileTypeShouldBeSavedInFileInfo()
        {
            var ev = new Event();
            ev.Id = 100;
            ev.Files = new List<FileInfo>();
            var fileInfo = new FileInfo(FileType.Thumbnail);
            fileInfo.Resolution = Resolution._128x128;
            ev.Files.Add(fileInfo);
            
            var modified = JsonConvert.DeserializeObject<Event>(JsonConvert.SerializeObject(ev));
            modified.Files.Clear();
            
            fileInfo = new FileInfo("some path", FileType.Thumbnail);
            modified.Files.Add(fileInfo);
            
            var deepComparingReq = new DifferenceDeepUpdateReq<Event>(ev, modified);
            deepComparingReq.BuildQueryObject(true);
            var result = deepComparingReq.QueryObject.ToString();

            var resEvent = JsonConvert.DeserializeObject<Event>(result);
            
            Assert.True(resEvent.Files.First().FileType == FileType.Thumbnail);
            Assert.IsTrue(deepComparingReq.HasDataToUpdate);
        }

        [Test]
        public void DeepDifferenceReq_ShouldExcludeFileInfo_WhenExcluded()
        {
            var origin = new FaceAnimation();
            origin.Files = new List<FileInfo>();
            origin.Files.Add(new FileInfo(Application.persistentDataPath, FileType.MainFile));
            origin.GroupId = 1;
            
            var modififed = JsonConvert.DeserializeObject<FaceAnimation>(JsonConvert.SerializeObject(origin));
            modififed.Files.First().TagAsModified();
            modififed.GroupId += 1;
            
            var optimizedReq = new DifferenceDeepUpdateReq<FaceAnimation>(origin, modififed);
            optimizedReq.BuildQueryObject(false);
            var queryText = optimizedReq.ToString();
            Assert.True(!queryText.Contains(nameof(IFilesAttachedEntity.Files).FirstCharToLower()));
            Assert.IsTrue(optimizedReq.HasDataToUpdate);
        }
    }
}
