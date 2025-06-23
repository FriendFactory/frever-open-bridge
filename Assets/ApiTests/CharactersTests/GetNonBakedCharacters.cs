using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.CharactersTests
{
    public class GetNonBakedCharacters: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetNonBakedCharacters(10);
            Debug.Log(JsonConvert.SerializeObject(resp));
            Debug.Log(resp.Models.Length);
        }
    }
}