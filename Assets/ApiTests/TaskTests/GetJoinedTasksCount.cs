using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.TaskTests
{
    internal sealed class GetJoinedTasksCount: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var res = await Bridge.GetJoinedVotingTasksCount();
            Debug.Log(JsonConvert.SerializeObject(res));
        }
    }
}