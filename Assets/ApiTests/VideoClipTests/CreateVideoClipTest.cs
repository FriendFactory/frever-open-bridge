using System;
using System.Collections.Generic;
using ApiTests;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;

public class CreateVideoClipTest : EntityApiTest<VideoClip>
{
    protected override async void RunTestAsync()
    {
        var videoClip = new VideoClip
        {
            Files = new List<FileInfo>()
        };

        var videoFile = new FileInfo(GetFilePath(TestFileNames.VIDEO_MP4), FileType.MainFile);
        videoClip.Files.Add(videoFile);

        await Bridge.PostAsync(videoClip);
    }
}
