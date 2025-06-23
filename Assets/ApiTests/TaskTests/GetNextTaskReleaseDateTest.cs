using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.TaskTests
{
    internal sealed class GetNextTaskReleaseDateTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var res = await Bridge.GetNextTaskReleaseDate();
            Debug.Log(JsonConvert.SerializeObject(res));
        }
    }
}