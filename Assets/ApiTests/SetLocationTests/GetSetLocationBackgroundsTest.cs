using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.SetLocationTests
{
    internal sealed class GetSetLocationBackgroundsTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var res = await Bridge.GetSetLocationBackgroundListAsync(10, 0);
            Debug.Log($"{JsonConvert.SerializeObject(res)}");
        }
    }
}