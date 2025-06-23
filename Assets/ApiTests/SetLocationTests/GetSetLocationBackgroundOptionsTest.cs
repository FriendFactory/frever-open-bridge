using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.SetLocationTests
{
    internal sealed class GetSetLocationBackgroundOptionsTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var res = await Bridge.GetSetLocationBackgroundOptionsAsync(10, 0);
            Debug.Log($"{JsonConvert.SerializeObject(res)}");
            Debug.Log($"{res.Model.Options.Count()}");
        }
    }
}