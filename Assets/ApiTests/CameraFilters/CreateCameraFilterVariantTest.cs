using System.Collections;
using System.Collections.Generic;
using ApiTests;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;
using UnityEngine;
using Resolution = Bridge.Models.Common.Files.Resolution;

public class CreateCameraFilterVariantTest : EntityApiTest<CameraFilterVariant>
{
    protected override async void RunTestAsync()
    {
        var cameraFilterVariant = new CameraFilterVariant();

        cameraFilterVariant.Files = new List<FileInfo>
        {
            new FileInfo(GetFilePath(TestFileNames.THUMBNAIL_PNG_1), FileType.Thumbnail, Resolution._128x128),
            new FileInfo(GetFilePath(TestFileNames.THUMBNAIL_PNG_1), FileType.Thumbnail, Resolution._256x256),
            new FileInfo(GetFilePath(TestFileNames.THUMBNAIL_PNG_1), FileType.Thumbnail, Resolution._512x512),
            new FileInfo(GetFilePath(TestFileNames.ASSET_BUNDLE), FileType.MainFile)
        };

        cameraFilterVariant.Name = "Serhii_" + Random.Range(0, 100000);
        cameraFilterVariant.ReadinessId = await GetAnyAvailableEntityId<Readiness>();
        cameraFilterVariant.CameraFilterId = await GetAnyAvailableEntityId<CameraFilter>();

        await Bridge.PostAsync(cameraFilterVariant);
    }
    
}
