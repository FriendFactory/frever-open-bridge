using ApiTests;
using Bridge.Models.AsseManager;
using Newtonsoft.Json;
using UnityEngine;

public class GetCameraFilterTest : EntityApiTest<CameraFilter>
{
    public long Id;
    protected override async void RunTestAsync()
    {
        var response = await Bridge.GetAsync<CameraFilter>(Id);
        if (response.IsSuccess)
        {
            Debug.Log(JsonConvert.SerializeObject(response.ResultObject));
        }
        else
        {
            Debug.LogError(response.ErrorMessage);
        }
    }
}
