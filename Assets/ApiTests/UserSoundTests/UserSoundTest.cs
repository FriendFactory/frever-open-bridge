using System.Collections.Generic;
using System.IO;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;
using UnityEngine;
using FileInfo = Bridge.Models.Common.Files.FileInfo;

namespace ApiTests.UserSoundTests
{
    public class UserSoundTest : EntityApiTest<UserSound>
    {
        protected override async void RunTestAsync()
        {
            var path = Path.Combine(Application.streamingAssetsPath, "TestFiles" ,TestFileNames.VIDEO_MP4);
            var videoBytes = File.ReadAllBytes(path);
            var res = await Bridge.ExtractAudioAsync(videoBytes, 15);
            if (res.IsError)
            {
                Debug.LogError(res.ErrorMessage);
                return;
            }

            var userSound = new UserSound
            {
                Files = new List<FileInfo> {new FileInfo(res.FilePath, FileType.MainFile)}
            };

            var response = await Bridge.PostAsync(userSound);
            
        }
    }
}
