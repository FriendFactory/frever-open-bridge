using ApiTests;
using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using Newtonsoft.Json;
using UnityEngine;

public class GetBrandList : EntityApiTest<Brand>
{
    protected override async void RunTestAsync()
    {
        var q = new Query<Brand>();
        q.SetSelectedFieldsNames(nameof(Brand.Name));
        var resp = await Bridge.GetAsync(q);
        Debug.Log(JsonConvert.SerializeObject(resp.Models));
    }
}
