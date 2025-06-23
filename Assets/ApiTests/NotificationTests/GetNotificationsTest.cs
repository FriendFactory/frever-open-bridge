using ApiTests;
using Newtonsoft.Json;
using UnityEngine;

public class GetNotificationsTest : AuthorizedUserApiTestBase
{
    protected override async void RunTestAsync()
    {
        var resp = await Bridge.MyLatestNotifications(20);
        if(resp.IsError)
            Debug.LogError(resp.ErrorMessage);
        else
            Debug.Log(JsonConvert.SerializeObject(resp.Models));
    }
}
