using System.Collections;
using System.Collections.Generic;
using ApiTests;
using Bridge.Models.AsseManager;
using UnityEngine;
using UnityEngine.UI;
using Resolution = Bridge.Models.Common.Files.Resolution;

public class DownloadCharacterSpawnPositionThumbnails : EntityApiTest<CharacterSpawnPositionFormation>
{
    protected override async void RunTestAsync()
    {
        var id = await GetAnyAvailableEntityId<CharacterSpawnPositionFormation>();
        var modelResp = await Bridge.GetAsync<CharacterSpawnPositionFormation>(id);
        var downloadResp = await Bridge.GetThumbnailAsync(modelResp.ResultObject, Resolution._128x128);
    }
}
