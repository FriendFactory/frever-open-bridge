using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.TaskTests
{
    internal sealed class GetListOfTasks : AuthorizedUserApiTestBase
    {
        public int Take = 10;
    
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetTasksAsync(null,Take, 0);
            if (resp.IsError)
            {
                Debug.LogError(resp.ErrorMessage);
            }
            else
            {
                Debug.Log(JsonConvert.SerializeObject(resp.Models));
                Debug.Log(resp.Models.Length);
            }
        }
    }
}
