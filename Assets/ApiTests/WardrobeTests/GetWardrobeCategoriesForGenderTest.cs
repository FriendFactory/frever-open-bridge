using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.WardrobeTests
{
    public class GetWardrobeCategoriesForGenderTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetWardrobeCategoriesPerGender();
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}