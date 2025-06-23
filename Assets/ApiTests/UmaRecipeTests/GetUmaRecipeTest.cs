using ApiTests;
using Bridge.Models.AsseManager;
using Newtonsoft.Json;
using UnityEngine;

public class GetUmaRecipeTest : EntityApiTest<UmaRecipe>
{
    protected override async void RunTestAsync()
    {
        var id = await GetAnyAvailableEntityId<UmaRecipe>();
        var resp = await Bridge.GetAsync<UmaRecipe>(id);
        Debug.Log(JsonConvert.SerializeObject(resp));
    }
}
