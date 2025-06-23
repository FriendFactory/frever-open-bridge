using System;
using System.Collections.Generic;
using ApiTests;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;

public class CreateVfx : EntityApiTest<Vfx>
{
    protected override async void RunTestAsync()
    {
        var vfx = new Vfx
        {
            VfxCategoryId = await GetAnyAvailableEntityId<VfxCategory>(),
            ReadinessId = await GetAnyAvailableEntityId<Readiness>(),
            UploaderUserId = Bridge.Profile.Id,
            VfxDirectionId = await GetAnyAvailableEntityId<VfxDirection>(),
            VfxTypeId = await GetAnyAvailableEntityId<VfxType>(),
            VfxWorldSizeId = await GetAnyAvailableEntityId<VfxWorldSize>(),
            Files = new List<FileInfo>()
            {
                new FileInfo(GetFilePath(TestFileNames.THUMBNAIL_GIF), FileType.Thumbnail, Resolution._128x128),
                new FileInfo(GetFilePath(TestFileNames.THUMBNAIL_GIF), FileType.Thumbnail, Resolution._256x256),
                new FileInfo(GetFilePath(TestFileNames.THUMBNAIL_GIF), FileType.Thumbnail, Resolution._512x512),
                new FileInfo(GetFilePath(TestFileNames.ASSET_BUNDLE), FileType.MainFile)
            },
            Name = Guid.NewGuid().ToString(),
            Tags = Array.Empty<long>()
        };

        var res = await Bridge.PostAsync(vfx);
        LogResult(res);
    }
}
