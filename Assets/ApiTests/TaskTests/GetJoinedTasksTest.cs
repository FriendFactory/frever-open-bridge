using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.TaskTests
{
    internal sealed class GetJoinedTasksTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var result = await Bridge.GetJoinedVotingTasks(null, 10, 10);
            Debug.Log(JsonConvert.SerializeObject(result));
        }
    }
}