using System.Collections.Generic;
using ApiTests;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;
using UnityEngine;

namespace ApiTests.SetLocationTests
{
    public class UpdateSetLocationThumbnail : EntityApiTest<SetLocation>
    {
        protected override async void RunTestAsync()
        {
            var anySetLocation = await this.GetAnyAvailableEntityId<SetLocation>();
            Debug.Log("SetLocation ID: " + anySetLocation);
            var thumbnailPath = GetFilePath(TestFileNames.THUMBNAIL_PNG_2);
            var res = await Bridge.UpdateFilesAsync<SetLocation>(anySetLocation, new List<FileInfo>()
            {
                new FileInfo(thumbnailPath, FileType.Thumbnail)
            });

            LogResult(res);
        }
    }
}