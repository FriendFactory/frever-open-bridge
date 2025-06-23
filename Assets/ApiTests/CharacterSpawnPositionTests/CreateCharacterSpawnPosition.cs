using System;
using System.Collections.Generic;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;

namespace ApiTests.CharacterSpawnPositionTests
{
    public class CreateCharacterSpawnPosition: EntityApiTest<CharacterSpawnPosition>
    {
        protected override async void RunTestAsync()
        {
            var model = new CharacterSpawnPosition();
            model.Files = new List<FileInfo>();
            model.Files.Add(new FileInfo(GetFilePath(TestFileNames.THUMBNAIL_PNG_1), FileType.Thumbnail, Resolution._128x128));
            model.Files.Add(new FileInfo(GetFilePath(TestFileNames.THUMBNAIL_PNG_1), FileType.Thumbnail, Resolution._256x256));
            model.Files.Add(new FileInfo(GetFilePath(TestFileNames.THUMBNAIL_PNG_1), FileType.Thumbnail, Resolution._512x512));
            model.Files.Add(new FileInfo(GetFilePath(TestFileNames.THUMBNAIL_PNG_1), FileType.Thumbnail, Resolution._1600x900));

            model.Name = Guid.NewGuid().ToString();
            model.SetLocationBundleId = await GetAnyAvailableEntityId<SetLocationBundle>();
            model.UnityGuid = Guid.NewGuid();

            var resp = await Bridge.PostAsync(model);
            LogResult(resp);
        }
    }
}